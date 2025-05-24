namespace MCComputersBackend.Service.Interface
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile file, string folder);
        Task<bool> DeleteFileAsync(string filePath);
        bool IsValidImageFile(IFormFile file);
        string GetFileUrl(string fileName);
    }
}
