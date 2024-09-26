namespace eBid.Auction.API.Model;

public class AuctionType
{
    public int Id { get; set; }

    [Required] public string Type { get; set; }
}