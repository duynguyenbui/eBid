namespace eBid.Auction.API.Services.Image;

public class ImageService : IImageService<ImageUploadResult, DeletionResult>
{
    private readonly IOptions<CloudinaryOptions> _options;
    private readonly Cloudinary _cloudinary;

    public ImageService(IOptions<CloudinaryOptions> options)
    {
        _options = options;

        var account = new Account(_options.Value.CloudName, _options.Value.ApiKey, _options.Value.ApiSecret);

        _cloudinary = new Cloudinary(account);
    }

    public async Task<ImageUploadResult> AddImageAsync(IFormFile file)
    {
        var uploadResult = new ImageUploadResult();

        if (file.Length > 0)
        {
            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams { File = new FileDescription(file.FileName, stream) };
            uploadResult = await _cloudinary.UploadAsync(uploadParams);
        }

        return uploadResult;
    }

    public async Task<DeletionResult> DeleteImageAsync(string publicId)
    {
        var deleteParams = new DeletionParams(publicId);

        var result = await _cloudinary.DestroyAsync(deleteParams);

        return result;
    }
}