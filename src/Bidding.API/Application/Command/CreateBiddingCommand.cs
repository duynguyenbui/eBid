namespace eBid.Bidding.API.Application.Command;

public record CreateBiddingCommand(int auctionId, decimal amount) : IRequest<bool>;