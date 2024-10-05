namespace eBid.Bidding.Domain.AggregatesModel.BiddingAggregate;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum BiddingStatus
{
    Accepted,
    AcceptedBelowReserve,
    TooLow,
    Finished
}