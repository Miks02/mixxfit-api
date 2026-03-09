using MixxFit.API.Common.Results;

namespace MixxFit.API.Common.Interfaces;

public interface IFileService
{
    Task<Result<string>> UploadFile(IFormFile file, string? uploadedFilePath, string? uploadSubDir);
    Result DeleteFile(string fileToDeletePath);
}