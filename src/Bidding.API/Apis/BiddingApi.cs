namespace eBid.Bidding.API.Apis;

public static class BiddingApi
{
    public static RouteGroupBuilder MapBiddingApiV1(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/bidding").HasApiVersion(1.0);
        api.MapGet("/items", () => "Get items");
        api.MapPost("/placebid", () => "Place bid");
        api.MapPost("/payment", () => "Payment");

        api.MapGet("/cardtypes",
            ([AsParameters] BiddingServices services) => TypedResults.Ok(services.Context.CardTypes.ToList()));

        return api;
    }
}

public record CreateBiddingRequest(
    string AuctionId,
    string Buyer,
    decimal Amount
);