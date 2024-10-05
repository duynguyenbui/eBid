namespace eBid.Bidding.API.Apis;

public class BiddingServices(
    IMediator mediator,
    BiddingContext context,
    IIdentityService identityService,
    ILogger<BiddingServices> logger,
    IBiddingRepository biddingRepository)
{
    public IMediator Mediator { get; } = mediator;
    public BiddingContext Context { get; } = context;
    public IBiddingRepository BiddingRepository { get; } = biddingRepository;
    public IIdentityService IdentityService { get; } = identityService;
    public ILogger<BiddingServices> Logger { get; } = logger;
}