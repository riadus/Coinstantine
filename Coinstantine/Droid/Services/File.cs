using System.IO;
using Android.Support.V4.Content;
using Coinstantine.Common.Attributes;
using Coinstantine.Domain.Interfaces;

namespace Coinstantine.Droid.Services
{
    [RegisterInterfaceAsDynamic]
    public class File : IFile
    {
        public bool Exists(string filePath)
        {
            return System.IO.File.Exists(filePath);
        }

        public byte[] ReadAllBytes(string filePath)
        {
            return System.IO.File.ReadAllBytes(filePath);
        }

        public string ReadAllText(string filePath)
        {
            return System.IO.File.ReadAllText(filePath);
        }

        public void WriteAllText(string filePath, string contents)
        {
            CreateDirectoryIfNeeded(filePath);
            System.IO.File.WriteAllText(filePath, contents);
        }

        private void CreateDirectoryIfNeeded(string filePath)
        {
            var directoryPath = Path.GetDirectoryName(filePath);
            Directory.CreateDirectory(directoryPath);
        }

        public void WriteAllBytes(string filePath, byte[] bytes)
        {
            CreateDirectoryIfNeeded(filePath);
            System.IO.File.WriteAllBytes(filePath, bytes);
        }

        public void DeleteFile(string filePath)
        {
            if (Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }
    }
}