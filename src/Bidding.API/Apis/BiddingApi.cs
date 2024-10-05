namespace eBid.Bidding.API.Apis;

public static class BiddingApi
{
    public static RouteGroupBuilder MapBiddingApiV1(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/bidding")
            .HasApiVersion(1.0);

        api.MapPost("/placebid", CreateBiddingAsync);
        api.MapGet("/bids/all/by/{auctionId:int}", GetBidsByAuctionIdAsync).AllowAnonymous();

        return api;
    }

    public static async Task<Results<Created, BadRequest<string>, ProblemHttpResult>> CreateBiddingAsync(
        [FromHeader(Name = "x-requestid")] Guid requestId, CreateBiddingCommand command,
        [AsParameters] BiddingServices services)
    {
        if (requestId == Guid.Empty)
        {
            return TypedResults.BadRequest("Empty GUID is not valid for request ID");
        }

        var createBiddingCommand = new IdentifiedCommand<CreateBiddingCommand, bool>(command, requestId);

        var commandResult = await services.Mediator.Send(createBiddingCommand);

        if (!commandResult)
        {
            return TypedResults.Problem(detail: "Cancel order failed to process.", statusCode: 500);
        }
        
        return TypedResults.Created();
    }

    public static async Task<Ok<List<Bid>>> GetBidsByAuctionIdAsync(
        [AsParameters] BiddingServices services, int auctionId)
    {
        var bids = await services.BiddingRepository.FindBidsByAuctionIdAsync(auctionId);

        return TypedResults.Ok(bids);
    }
}