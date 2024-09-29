namespace eBid.Auction.API.Services;

public class AuctionServices(
    AuctionContext context,
    IOptions<AuctionOptions> options,
    ILogger<AuctionServices> logger,
    IAuctionIntegrationEventService eventService,
    IAuctionAI auctionAI,
    IImageService<ImageUploadResult, DeletionResult> imageService,
    IIdentityService identityService)
{
    public AuctionContext Context { get; } = context;
    public IOptions<AuctionOptions> Options { get; } = options;
    public ILogger<AuctionServices> Logger { get; } = logger;
    public IAuctionIntegrationEventService EventService { get; } = eventService;
    public IAuctionAI AuctionAI { get; } = auctionAI;
    public IImageService<ImageUploadResult, DeletionResult> ImageService { get; } = imageService;
    public IIdentityService IdentityService { get; } = identityService;
}