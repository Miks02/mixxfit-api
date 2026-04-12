using MixxFit.API.Common.Results;

namespace MixxFit.API.Common.Interfaces;

public interface IFileService
{
    Task<Result<string>> UploadFile(IFormFile file, string? uploadedFilePath, string? uploadSubDir);
    Task<Result> DeleteFile(string fileToDeletePath);
}