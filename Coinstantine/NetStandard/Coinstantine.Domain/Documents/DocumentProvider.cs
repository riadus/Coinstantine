using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coinstantine.Common;
using Coinstantine.Common.Attributes;
using Coinstantine.Data;
using Coinstantine.Domain.Interfaces;
using Coinstantine.Domain.Mapping.DTOs;

namespace Coinstantine.Domain.Documents
{
    [RegisterInterfaceAsDynamic]
    public class DocumentProvider : IDocumentProvider
    {
        private readonly IBackendService _backendService;
        private readonly IPathProvider _pathProvider;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFile _file;

        public DocumentProvider(IBackendService backendService,
                                IPathProvider pathProvider,
                                IUnitOfWork unitOfWork,
                                IFile file)
        {
            _backendService = backendService;
            _pathProvider = pathProvider;
            _unitOfWork = unitOfWork;
            _file = file;
        }

        public async Task<bool> DownloadDocuments()
        {
            try
            {
                var currentDocuments = await _unitOfWork.Documents.GetAsync().ConfigureAwait(false);
                var onlineDocuments = await _backendService.GetDocuments();
                foreach (var documentsToUpdate in NeedToUpdateDocuments(currentDocuments, onlineDocuments))
                {
                    if (!documentsToUpdate.DocumentVersion.DocumentAvailable)
                    {
                        var path = GetPath(documentsToUpdate.Document?.DocumentType);
                        _file.DeleteFile(path);
                        await _unitOfWork.Documents.DeleteAsync(x => x.DocumentType == documentsToUpdate.DocumentVersion.DocumentType).ConfigureAwait(false);
                    }
                    Document document = null;
                    if (documentsToUpdate.DocumentVersion.FileType.ApplicationType == FileApplicationType.Web)
                    {
                        document = new Document
                        {
                            DocumentAvailable = documentsToUpdate.DocumentVersion.DocumentAvailable,
                            DocumentType = documentsToUpdate.DocumentVersion.DocumentType,
                            Filename = documentsToUpdate.DocumentVersion.Filename,
                            FileType = documentsToUpdate.DocumentVersion.FileType.ApplicationType,
                            LastModifiedDate = documentsToUpdate.DocumentVersion.LastModifiedDate ?? new System.DateTime()
                        };
                    }
                    else if (documentsToUpdate.NeedToUpdate && documentsToUpdate.DocumentVersion.DocumentAvailable)
                    {
                        document = await _backendService.DownloadDocument(documentsToUpdate.DocumentVersion).ConfigureAwait(false);
                        var path = GetPath(document.DocumentType);
                        _file.WriteAllBytes(path, document.Bytes);
                    }
                    if (document != null)
                    {
                        await _unitOfWork.Documents.DeleteAsync(x => x.DocumentType == document.DocumentType).ConfigureAwait(false);
                        await _unitOfWork.Documents.SaveAsync(document).ConfigureAwait(false);
                    }
                }
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        private IEnumerable<(DocumentVersion DocumentVersion, Document Document , bool NeedToUpdate)> NeedToUpdateDocuments(IEnumerable<Document> currentDocuments, IEnumerable<DocumentVersion> versions)
        {
            foreach (var version in versions)
            {
                var relatedDocument = currentDocuments?.FirstOrDefault(doc => doc.DocumentType == version.DocumentType);
                if (relatedDocument == null)
                {
                    yield return (version, null, true);
                }
                else
                {
                    yield return (version, relatedDocument, version.LastModifiedDate != relatedDocument.LastModifiedDate || relatedDocument.Bytes == null);
                }
            }
        }

        public Task<IEnumerable<Document>> LoadDocuments() 
        {
            return _unitOfWork.Documents.GetAsync();
        }

        public Document LoadFullDocument(Document document)
        {
            var path = GetPath(document.DocumentType);
            if (_file.Exists(path))
            {
                var bytes = _file.ReadAllBytes(path);
                document.Path = path;
                document.Bytes = bytes;
            }
            return document;
        }

        private string GetPath(DocumentType? documentType)
        {
            switch(documentType)
            {
                case DocumentType.WhitePaper:
                    return _pathProvider.WhitePaperPath;
                case DocumentType.TermsAndServices:
                    return _pathProvider.TermsAndServicesPath;
                case DocumentType.PrivacyPolicy:
                    return _pathProvider.PrivacyPolicyPath;
            }
            return string.Empty;
        }

    }
}
