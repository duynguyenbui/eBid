namespace eBid.Auction.API.Model;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AuctionStatus
{
    Live,
    Finished,
    ReserveNotMet
}