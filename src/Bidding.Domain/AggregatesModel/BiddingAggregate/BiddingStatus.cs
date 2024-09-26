namespace eBid.Bidding.Domain.AggregatesModel.BiddingAggregate;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum BiddingStatus
{
    Live,
    Finished,
    ReserveNotMet
}