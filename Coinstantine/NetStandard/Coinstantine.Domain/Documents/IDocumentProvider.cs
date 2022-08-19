using System.Collections.Generic;
using System.Threading.Tasks;
using Coinstantine.Data;

namespace Coinstantine.Domain.Documents
{
    public interface IDocumentProvider
    {
        Task<bool> DownloadDocuments();
        Task<IEnumerable<Document>> LoadDocuments();
        Document LoadFullDocument(Document document);
    }
}
