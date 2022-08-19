namespace Coinstantine.Domain.Interfaces
{
    public interface IFile
    {
        bool Exists(string filePath);
        string ReadAllText(string filePath);
        byte[] ReadAllBytes(string filePath);
        void WriteAllBytes(string filePath, byte[] bytes);
        void WriteAllText(string filePath, string contents);
        void DeleteFile(string filePath);
    }
}