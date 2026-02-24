using MixxFit.VSA.Common.Results;

namespace MixxFit.VSA.Common.Interfaces;

public interface IFileService
{
    Task<Result<string>> UploadFile(IFormFile file, string? uploadedFilePath, string? uploadSubDir);
    Result DeleteFile(string fileToDeletePath);
}