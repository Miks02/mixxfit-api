

using MixxFit.VSA.Common.Interfaces;
using MixxFit.VSA.Common.Results;
using MixxFit.VSA.Domain.ErrorCatalog;

namespace MixxFit.VSA.Infrastructure.Storage;

public class LocalFileStorage(IWebHostEnvironment web, ILogger<LocalFileStorage> logger) : IFileService
{
    public async Task<Result<string>> UploadFile(IFormFile file, string? uploadedFilePath, string? uploadSubDir)
    { 
        var fileValidationResult = IsFileValid(file);

        if (!fileValidationResult.IsSuccess)
        {
            logger.LogWarning("{error}", fileValidationResult.Errors[0].Description);
            return Result<string>.Failure(FileError.ValidationFailed());
        }

        var uploadDir = "uploads";

        if (!string.IsNullOrEmpty(uploadSubDir))
        {
            uploadSubDir = uploadSubDir.TrimStart('/');
            uploadDir = Path.Combine(uploadDir, uploadSubDir).Replace('\\', '/') + '/';
        }

        var uploadsDirPath = Path.Combine(web.WebRootPath, uploadDir.TrimStart());

        if (!Directory.Exists(uploadsDirPath))
            Directory.CreateDirectory(uploadsDirPath);

        var sanitizedFileName = Path.GetFileName(file.FileName);
        var uniqueFileName = $"{Guid.NewGuid()}_{sanitizedFileName}";
        var filePath = Path.Combine(uploadsDirPath, uniqueFileName);

        if (!string.IsNullOrEmpty(uploadedFilePath))
            DeleteFile(uploadedFilePath);

        await using var fileStream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(fileStream);

        return Result<string>.Success(uploadDir + uniqueFileName);

    }

    public Result DeleteFile(string filePath)
    {
        var oldFilePath = Path.Combine(web.WebRootPath,
            filePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

        if (!File.Exists(oldFilePath))
        {
            logger.LogWarning("File does not exist");
            Result.Failure(GeneralError.NotFound(filePath));
        }

        try
        {
            File.Delete(oldFilePath);
        }
        catch (IOException ex)
        {
            logger.LogError("Unexpected error happened while trying to delete the file. {ex}", ex);
            throw;
        }

        return Result.Success();
    }

    private Result IsFileValid(IFormFile file)
    {
        var fileSize = file.Length;

        if (fileSize == 0)
            return Result.Failure(FileError.Empty());

        var maxFileSize = 5 * 1024 * 1024;

        if (fileSize > maxFileSize)
            return Result.Failure(FileError.TooLarge(maxFileSize));

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (!allowedExtensions.Contains(extension))
            return Result.Failure(FileError.UnsupportedExtension(extension));
        
        return Result.Success();

    }    
}