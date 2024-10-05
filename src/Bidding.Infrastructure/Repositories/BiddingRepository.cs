using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace eBid.Bidding.Infrastructure.Repositories;

public class BiddingRepository(BiddingContext context) : IBiddingRepository
{
    public Bid? Add(Bid bid)
    {
        var biddingEntry = context.Add(bid);

        var changes = context.SaveChanges();

        return changes == 0 ? null : biddingEntry.Entity;
    }

    public async Task<AuctionItem?> FindByAuctionIdAsync(int id)
    {
        var item = await context.AuctionItems
            .FirstOrDefaultAsync(a => a.Id == id);

        return item;
    }

    public async Task<Bid?> GetHighestBidAsync(int auctionId)
    {
        var bid = await context.Bids
            .Where(b => b.AuctionItemId == auctionId)
            .OrderByDescending(b => b.Amount)
            .FirstOrDefaultAsync();

        return bid;
    }

    public async Task<List<Bid>> FindBidsByAuctionIdAsync(int auctionId)
    {
        var bids = await context.Bids
            .Where(b => b.AuctionItemId == auctionId)
            .OrderByDescending(b => b.BidTime)
            .ToListAsync();

        return bids;
    }

    public Task<List<Bid>> FindAsync(string biddingIdentityGuid)
    {
        throw new NotImplementedException();
    }
}