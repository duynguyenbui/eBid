namespace eBid.Bidding.Domain.AggregatesModel.BiddingAggregate;

public class AuctionItem
{
    public int Id { get; set; }
    public DateTime AuctionEnd { get; set; }
    public required string Seller { get; set; }
    public decimal StartingPrice { get; set; }
    public bool Finished { get; set; }
}