namespace eBid.BiddingProcessor.Events;

public record AuctionStatusChangedToFinished : IntegrationEvent
{
    public int AuctionId { get; set; }

    public AuctionStatusChangedToFinished(int auctionId)
    {
        AuctionId = auctionId;
    }
}