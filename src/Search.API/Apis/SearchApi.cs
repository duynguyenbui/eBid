namespace eBid.Search.API;

public static class SearchApi
{
    private const string Index = "auctions";

    public static IEndpointRouteBuilder MapSearchApiV1(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/search").HasApiVersion(1.0);

        api.MapGet("/items/all", GetAllItems);
        api.MapGet("/items/by/id/{id:int}", GetItemById);
        api.MapGet("/items/by/ids", GetItemByIds);
        api.MapGet("/items/type/{typeId}", GetItemsByTypeId);
        api.MapGet("/items/by/auctionstatus", GetItemsByAuctionStatus);
        api.MapGet("/items/by/name/{term:minlength(1)}", GetItemsByName);
        api.MapGet("/by/auctiontypes", GetAuctionTypes);

        return app;
    }

    private static async Task<Ok<PaginatedItems<AuctionItemData>>> GetItemByIds(
        [AsParameters] SearchServices services, [AsParameters] PaginationRequest request, int[] ids)
    {
        var result = await services.EsRepository.GetItemsByIds(Index, ids, request.From, request.Size);

        return TypedResults.Ok(new PaginatedItems<AuctionItemData>(request.From, request.Size, result.Count, result));
    }

    private static async Task<Results<Ok<PaginatedItems<AuctionItemData>>, BadRequest>> GetItemsByAuctionStatus(
        [AsParameters] SearchServices services,
        [AsParameters] PaginationRequest request,
        string status)
    {
        if (status is not ("Live" or "Finished" or "ReserveNotMet"))
        {
            return TypedResults.BadRequest();
        }

        var result = await services.EsRepository.GetItemsByStatus(Index, status.ToString(),
            request.From, request.Size);

        return TypedResults.Ok(new PaginatedItems<AuctionItemData>(request.From, request.Size, result.Count, result));
    }

    private static async Task<Ok<PaginatedItems<AuctionItemData>>> GetItemsByTypeId(
        [AsParameters] SearchServices services, [AsParameters] PaginationRequest request, int typeId)
    {
        var result = await services.EsRepository.GetItemsByTypeId(Index, typeId, request.From, request.Size);

        return TypedResults.Ok(new PaginatedItems<AuctionItemData>(request.From, request.Size, result.Count, result));
    }

    private static async Task<Ok<AuctionItemData>> GetItemById([AsParameters] SearchServices services, int id)
    {
        var result = await services.EsRepository.GetItemById(Index, id);
        return TypedResults.Ok(result);
    }

    private static async Task<IResult> GetAuctionTypes([AsParameters] SearchServices services,
        [AsParameters] PaginationRequest request)
    {
        var result = await services.EsRepository.GetTypes(Index, request.From, request.Size);
        return TypedResults.Ok(result);
    }


    private static async Task<Results<Ok<PaginatedItems<AuctionItemData>>, NotFound>> GetItemsByName(
        [AsParameters] SearchServices services,
        [AsParameters] PaginationRequest request,
        string term)
    {
        var from = request.From;
        var size = request.Size;

        var result = await services.EsRepository.GetItemsByName(Index, term, from, size);

        if (result.Count == 0) return TypedResults.NotFound();

        return TypedResults.Ok(new PaginatedItems<AuctionItemData>(from, size, result.Count, result));
    }

    private static async Task<Ok<PaginatedItems<AuctionItemData>>> GetAllItems([AsParameters] SearchServices services,
        [AsParameters] PaginationRequest request)
    {
        var response = await services.EsRepository
            .GetAllItems("auctions", request.From, request.Size);

        return TypedResults.Ok(
            new PaginatedItems<AuctionItemData>(request.From, request.Size, response.Count, response));
    }
}