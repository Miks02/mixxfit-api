using CloudinaryDotNet;

namespace MixxFit.API.Infrastructure.Cloudinary
{
    public static class CloudinaryRegistration
    {
        public static void AddCloudinary(this IServiceCollection services, IConfiguration configuration)
        {
            var cloudinarySettings = configuration.GetSection("CloudinarySettings").Get<CloudinaryOptions>();
            var account = new Account(
                cloudinarySettings!.CloudName,
                cloudinarySettings.ApiKey,
                cloudinarySettings.ApiSecret
            );
            var cloudinary = new CloudinaryDotNet.Cloudinary(account);

            services.AddSingleton(cloudinary);
        }
    }
}
