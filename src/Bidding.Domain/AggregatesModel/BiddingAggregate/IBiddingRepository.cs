namespace eBid.Bidding.Domain.AggregatesModel.BiddingAggregate;

public interface IBiddingRepository
{
    Bid? Add(Bid bid);
    Task<AuctionItem?> FindByAuctionIdAsync(int id);
    Task<Bid?> GetHighestBidAsync(int auctionId);
    Task<List<Bid>> FindBidsByAuctionIdAsync(int auctionId);
    Task<List<Bid>> FindAsync(string guidIdentityId);
}