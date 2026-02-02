using VitalOps.API.Services.Results;

namespace VitalOps.API.Services.Interfaces
{
    public interface IFileService
    {
        Task<Result<string>> UploadFile(IFormFile file, string? uploadedFilePath, string? uploadSubDir);
        Result DeleteFile(string fileToDeletePath);
    }
}
