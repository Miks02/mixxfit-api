using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Common.Results;
using MixxFit.API.Domain.ErrorCatalog;

namespace MixxFit.API.Infrastructure.Storage;

public class CloudinaryFileStorage(Cloudinary cloudinary, ILogger<CloudinaryFileStorage> logger) : IFileService
{
    public async Task<Result<string>> UploadFile(IFormFile file, string? uploadedFilePath, string? uploadSubDir)
    {
        var fileValidationResult = IsFileValid(file);
        if (!fileValidationResult.IsSuccess)
            return Result<string>.Failure(fileValidationResult.Errors[0]);

        if (!string.IsNullOrEmpty(uploadedFilePath))
            await DeleteFile(uploadedFilePath);

        await using var stream = file.OpenReadStream();
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            Folder = uploadSubDir ?? "mixxfit-uploads",
            Transformation = new Transformation().Quality("auto").FetchFormat("auto")
        };

        var uploadResult = await cloudinary.UploadAsync(uploadParams);

        if (uploadResult.Error != null)
        {
            logger.LogError("Cloudinary Error: {error}", uploadResult.Error.Message);
            return Result<string>.Failure(FileError.ValidationFailed());
        }

        return Result<string>.Success(uploadResult.SecureUrl.ToString());
    }

    public async Task<Result> DeleteFile(string filePath)
    {
        var publicId = ExtractPublicIdFromUrl(filePath);

        if (string.IsNullOrEmpty(publicId)) return Result.Success();

        var deleteParams = new DeletionParams(publicId);
        var result = await cloudinary.DestroyAsync(deleteParams);

        if (result.Result != "ok")
            logger.LogWarning("Cloudinary delete failed for ID: {id}", publicId);

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

    private string? ExtractPublicIdFromUrl(string url)
    {
        if (string.IsNullOrEmpty(url) || !url.Contains("res.cloudinary.com")) return null;

        var uri = new Uri(url);
        var pathSegments = uri.AbsolutePath.Split('/');
        var fileNameWithExtension = pathSegments.Last();
        var fileName = Path.GetFileNameWithoutExtension(fileNameWithExtension);

        var folder = pathSegments[pathSegments.Length - 2];
        return folder != "upload" ? $"{folder}/{fileName}" : fileName;
    }

    Result IFileService.DeleteFile(string filePath)
    {
        DeleteFile(filePath).Wait();
        return Result.Success();
    }
}