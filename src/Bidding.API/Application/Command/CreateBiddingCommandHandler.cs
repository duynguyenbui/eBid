namespace eBid.Bidding.API.Application.Command;

public class CreateBiddingCommandHandler : IRequestHandler<CreateBiddingCommand, bool>
{
    private readonly IBiddingRepository _repository;
    private readonly IIdentityService _identityService;
    private readonly GrpcClient _grpcClient;
    private readonly ILogger<CreateBiddingCommandHandler> _logger;

    public CreateBiddingCommandHandler(IBiddingRepository repository,
        GrpcClient grpcClient,
        ILogger<CreateBiddingCommandHandler> logger, IIdentityService identityService)
    {
        _repository = repository;
        _grpcClient = grpcClient;
        _logger = logger;
        _identityService = identityService;
    }

    /// <summary>
    /// Handler which processes the command when
    /// customer executes cancel order from app
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<bool> Handle(CreateBiddingCommand request, CancellationToken cancellationToken)
    {
        var auction = await _repository.FindByAuctionIdAsync(request.auctionId)
                      ?? _grpcClient.GetGrpcAuction(request.auctionId);

        if (auction is null)
        {
            _logger.LogWarning("Auction with id: {AuctionId} was not found", request.auctionId);
            return false;
        }

        if (auction.Seller == _identityService.GetUserIdentity())
        {
            _logger.LogWarning("Seller cannot bid on their own auction: {AuctionId}", request.auctionId);
            return false;
        }

        var bid = new Bid
        {
            Amount = request.amount,
            AuctionItemId = auction.Id,
            Bidder = _identityService.GetUserIdentity(),
            AuctionItem = auction
        };

        if (auction.AuctionEnd < DateTime.UtcNow)
        {
            bid.Status = BiddingStatus.Finished;
        }
        else
        {
            var highestBid = await _repository.GetHighestBidAsync(request.auctionId);

            if (highestBid != null && request.amount > highestBid.Amount || highestBid == null)
            {
                bid.Status = request.amount > auction.StartingPrice
                    ? BiddingStatus.Accepted
                    : BiddingStatus.AcceptedBelowReserve;
            }

            if (highestBid != null && bid.Amount <= highestBid.Amount)
            {
                bid.Status = BiddingStatus.TooLow;
            }
        }

        var created = _repository.Add(bid);

        if (created != null)
        {
            return true;
        }

        _logger.LogWarning("Failed to create bid for auction: {AuctionId}", request.auctionId);
        return false;
    }
}

public class CreateBiddingIdentifierCommandHandler : IdentifiedCommandHandler<CreateBiddingCommand, bool>
{
    public CreateBiddingIdentifierCommandHandler(IMediator mediator, IRequestManager requestManager,
        ILogger<IdentifiedCommandHandler<CreateBiddingCommand, bool>> logger) : base(mediator, requestManager, logger)
    {
    }

    protected override bool CreateResultForDuplicateRequest()
    {
        return true; // Ignore duplicate requests for processing order.
    }
}