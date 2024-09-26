namespace eBid.Auction.API.Model.DataTransferObjects;

public class AuctionCreatedDataTransferObject
{
    public string Name { get; set; }

    public string Description { get; set; }

    public decimal StartingPrice { get; set; }

    public DateTime EndingTime { get; set; }

    public int AuctionTypeId { get; set; }
}