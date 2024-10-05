namespace eBid.Bidding.Domain.AggregatesModel.BiddingAggregate;

public class Bid
{
    public int Id { get; set; }
    public string Bidder { get; set; }
    public DateTime BidTime { get; set; } = DateTime.UtcNow;
    public decimal Amount { get; set; }
    public BiddingStatus Status { get; set; }

    public int AuctionItemId { get; set; }
    public AuctionItem AuctionItem { get; set; }
}