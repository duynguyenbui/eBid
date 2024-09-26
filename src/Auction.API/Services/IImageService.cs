namespace eBid.Auction.API.Services;

public interface IImageService<TUploadResult, TDeleteResult>
{
    public Task<TUploadResult> AddImageAsync(IFormFile image);
    public Task<TDeleteResult> DeleteImageAsync(string publicId);
}