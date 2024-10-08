using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace eBid.Auction.API;

public static class AuctionApi
{
    public static void MapAuctionApiV1(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/auction").HasApiVersion(1.0);

        // All GET routes will be removed in production mode in the future because search services will take a responsibility to handle search functions

        // Routes for querying auction items
        api.MapGet("/items", GetAllItems);
        api.MapGet("/items/by", GetItemByIds);
        api.MapGet("/items/{id:int}", GetItemById);
        api.MapGet("/items/by/{name:minlength(1)}", GetItemsByName);

        // Routes for resolving auction items by types and other filters
        api.MapGet("/items/type/{typeId}", GetItemsByTypeId);
        api.MapGet("/auctionstatus",
            async (AuctionContext context, [FromQuery] AuctionStatus status) =>
            await context.AuctionItems.Where(x => x.Status == status).ToListAsync());
        api.MapGet("/auctiontypes",
            async (AuctionContext context) => await context.AuctionTypes.OrderBy(x => x.Id).ToListAsync());

        // Route for using resolving auction items using AI 
        api.MapGet("/items/withsemanticrelevance/{text:minlength(1)}", GetItemsBySemanticRelevance)
            .RequireAuthorization();

        // Route for handling unstructured data such as images
        api.MapPost("/items/image/{id:int}", UploadItemImage).DisableAntiforgery().RequireAuthorization();

        // Route for modifying auction items
        api.MapPost("/items/type", CreateType).RequireAuthorization();
        api.MapPost("/items", CreateItem).RequireAuthorization();
        api.MapPut("/items", UpdateItem).RequireAuthorization();
        api.MapDelete("/items/{id:int}", DeleteItemById).RequireAuthorization();

        // Route to change state of auction items
        api.MapPost("/items/{id:int}/state/to/sell", ChangeState).RequireAuthorization();
    }

    private static async Task<Results<Created, BadRequest<string>>> UploadItemImage(
        [AsParameters] AuctionServices services,
        int id, IFormFile image)
    {
        if (!IsImageFile(image)) return TypedResults.BadRequest("Invalid image file type");

        var item = await services.Context.AuctionItems.FindAsync(id);

        if (item is null)
            return TypedResults.BadRequest("Item not found");

        if (!item.IsAllowedToUpdate())
            return TypedResults.BadRequest("Item is not allowed to be updated due to some of the reasons");

        if (item.SellerId != services.IdentityService.GetUserIdentity())
            return TypedResults.BadRequest("You are not the owner of this item.");

        if (item.PicturePublicId != null)
            await services.ImageService.DeleteImageAsync(item.PicturePublicId);

        var result = await services.ImageService.AddImageAsync(image);

        if (result.Error != null) return TypedResults.BadRequest(result.Error.Message);

        // Update image of this auction
        item.PicturePublicId = result.PublicId;
        item.PictureUrl = result.SecureUrl.ToString();

        await services.Context.SaveChangesAsync();

        // Publish to Message Broker
        var auctionItemUpdatedEvent = new AuctionImageUpdatedIntegrationEvent(id, result.SecureUrl.ToString());

        await services.EventService.SaveEventAndCatalogContextChangesAsync(auctionItemUpdatedEvent);
        await services.EventService.PublishThroughEventBusAsync(auctionItemUpdatedEvent);

        return TypedResults.Created();
    }

    private static async Task<Results<Ok, BadRequest<string>>> ChangeState(
        [AsParameters] AuctionServices services, int id)
    {
        var item = await services.Context.AuctionItems.FindAsync(id);
        if (item is null)
        {
            return TypedResults.BadRequest("Item not found");
        }

        if (item.OnSell)
        {
            return TypedResults.BadRequest("Item is already on sell");
        }

        if (item.PictureUrl == null || item.PicturePublicId == null)
        {
            return TypedResults.BadRequest("Item does not have an image");
        }

        // Make sure that it is idempotent so we don't end up in a loop
        item.OnSell = true;
        await services.Context.SaveChangesAsync();

        var auctionChangedToBeSoldIntegrationEvent = new AuctionChangedToBeSoldIntegrationEvent(item.Id);

        await services.EventService.SaveEventAndCatalogContextChangesAsync(auctionChangedToBeSoldIntegrationEvent);
        await services.EventService.PublishThroughEventBusAsync(auctionChangedToBeSoldIntegrationEvent);

        return TypedResults.Ok();
    }

    private static async Task<Results<BadRequest<string>, RedirectToRouteHttpResult, Ok<PaginatedItems<AuctionItem>>>>
        GetItemsBySemanticRelevance(
            [AsParameters] PaginationRequest paginationRequest,
            [AsParameters] AuctionServices services,
            string text)
    {
        var pageSize = paginationRequest.PageSize;
        var pageIndex = paginationRequest.PageIndex;

        if (!services.AuctionAI.IsEnabled)
        {
            return await GetItemsByName(paginationRequest, services, text);
        }

        // Create an embedding for the input search
        var vector = await services.AuctionAI.GetEmbeddingAsync(text);

        // Get the total number of items
        var totalItems = await services.Context.AuctionItems
            .LongCountAsync();

        // Get the next page of items, ordered by most similar (smallest distance) to the input search
        List<AuctionItem> itemsOnPage;
        var itemsWithDistance = await services.Context.AuctionItems
            .Select(c => new { Item = c, Distance = c.Embedding.CosineDistance(vector) })
            .OrderBy(c => c.Distance)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync();

        services.Logger.LogDebug("Results from {text}: {results}", text,
            string.Join(", ", itemsWithDistance.Select(i => $"{i.Item.Name} => {i.Distance}")));

        itemsOnPage = itemsWithDistance.Select(i => i.Item).ToList();

        return TypedResults.Ok(new PaginatedItems<AuctionItem>(pageIndex, pageSize, totalItems, itemsOnPage));
    }

    private static async Task<Results<Created, NotFound<string>>> UpdateItem(
        [AsParameters] AuctionServices services,
        AuctionUpdatedDataTransferObject productToUpdate)
    {
        var auctionItem = await services.Context.AuctionItems
            .Include(item => item.AuctionType)
            .SingleOrDefaultAsync(i => i.Id == productToUpdate.Id);

        if (auctionItem == null)
            return TypedResults.NotFound($"Item with id {productToUpdate.Id} not found.");

        if (!auctionItem.IsAllowedToUpdate())
            return TypedResults.NotFound("Item is not allowed to be updated due to some restrictions.");

        var sellerId = auctionItem.SellerId ?? string.Empty;

        if (sellerId != services.IdentityService.GetUserIdentity())
            return TypedResults.NotFound("You are not the owner of this item.");

        var auctionEntry = services.Context.Entry(auctionItem);
        auctionEntry.CurrentValues.SetValues(productToUpdate);

        auctionItem.UpdatedAt = DateTime.UtcNow;

        if (services.AuctionAI.IsEnabled)
        {
            auctionItem.Embedding = await services.AuctionAI.GetEmbeddingAsync(auctionItem)
                                    ?? throw new Exception("Failed to generate embedding.");
        }

        await services.Context.SaveChangesAsync();

        var auctionItemUpdatedIntegrationEvent =
            new AuctionUpdatedIntegrationEvent(auctionEntry.Entity.ToAuctionItemData());

        await services.EventService.SaveEventAndCatalogContextChangesAsync(auctionItemUpdatedIntegrationEvent);
        await services.EventService.PublishThroughEventBusAsync(auctionItemUpdatedIntegrationEvent);

        return TypedResults.Created($"/api/auction/items/{productToUpdate.Id}");
    }


    private static async Task<Results<Created, BadRequest>> CreateType([AsParameters] AuctionServices services,
        [FromBody] AuctionTypeCreatedDataTransferObject auctionType)
    {
        var existedType = await services.Context.AuctionTypes.Where(at => at.Type == auctionType.Type)
            .FirstOrDefaultAsync();

        if (existedType is not null)
        {
            return TypedResults.BadRequest();
        }

        var eType = await services.Context.AuctionTypes.AddAsync(new AuctionType() { Type = auctionType.Type });
        await services.Context.SaveChangesAsync();

        return TypedResults.Created($"/api/auction/auctiontypes/{eType.Entity.Id}");
    }

    private static async Task<Results<NoContent, NotFound, BadRequest<string>>> DeleteItemById(
        [AsParameters] AuctionServices services,
        int id)
    {
        var item = services.Context.AuctionItems.SingleOrDefault(x => x.Id == id);

        if (item is null)
        {
            return TypedResults.NotFound();
        }

        if (item.OnSell)
        {
            return TypedResults.BadRequest("Item is on sell, cannot be deleted.");
        }

        services.Context.AuctionItems.Remove(item);
        await services.Context.SaveChangesAsync();

        // Delete image from Cloudinary
        if (!string.IsNullOrEmpty(item.PicturePublicId))
            await services.ImageService.DeleteImageAsync(item.PicturePublicId);

        var auctionItemDeletedEvent = new AuctionDeletedIntegrationEvent(id);
        await services.EventService.SaveEventAndCatalogContextChangesAsync(auctionItemDeletedEvent);
        await services.EventService.PublishThroughEventBusAsync(auctionItemDeletedEvent);

        return TypedResults.NoContent();
    }

    private static async Task<Results<Created, NotFound>> CreateItem([AsParameters] AuctionServices services,
        AuctionCreatedDataTransferObject data)
    {
        var type = await services.Context.AuctionTypes
            .Where(at => at.Id == data.AuctionTypeId)
            .FirstOrDefaultAsync();

        if (type == null) return TypedResults.NotFound();

        var item = new AuctionItem
        {
            AuctionType = type,
            Description = data.Description,
            Name = data.Name,
            OnSell = false,
            SellerId = services.IdentityService.GetUserIdentity(),
            StartingPrice = data.StartingPrice,
            EndingTime = data.EndingTime,
        };

        if (services.AuctionAI.IsEnabled)
        {
            item.Embedding = await services.AuctionAI.GetEmbeddingAsync(item)
                             ?? throw new Exception("Failed to generate embedding.");
        }

        var entry = services.Context.Add(item);
        await services.Context.SaveChangesAsync();

        // Publish to Message Broker
        var auctionItemCreatedEvent = new AuctionCreatedIntegrationEvent(entry.Entity.ToAuctionItemData());
        await services.EventService.SaveEventAndCatalogContextChangesAsync(auctionItemCreatedEvent);
        await services.EventService.PublishThroughEventBusAsync(auctionItemCreatedEvent);

        return TypedResults.Created($"/api/auction/items/{item.Id}");
    }

    private static async Task<Results<Ok<PaginatedItems<AuctionItem>>, BadRequest<string>>> GetItemsByTypeId(
        int typeId,
        [AsParameters] PaginationRequest request,
        [AsParameters] AuctionServices services)
    {
        var pageSize = request.PageSize;
        var pageIndex = request.PageIndex;

        var root = (IQueryable<AuctionItem>)services.Context.AuctionItems;

        root = root.Where(c => c.AuctionTypeId == typeId);

        var totalItems = await root.LongCountAsync();

        var itemsOnPage = await root
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync();

        return TypedResults.Ok(new PaginatedItems<AuctionItem>(pageIndex, pageSize, totalItems, itemsOnPage));
    }

    private static async Task<Ok<PaginatedItems<AuctionItem>>> GetItemsByName([AsParameters] PaginationRequest request,
        [AsParameters] AuctionServices services,
        string name)
    {
        var pageSize = request.PageSize;
        var pageIndex = request.PageIndex;

        var totalItems = await services.Context.AuctionItems
            .Where(item => item.Name.StartsWith(name))
            .LongCountAsync();

        var itemsOnPage = await services.Context.AuctionItems
            .Where(c => c.Name.StartsWith(name))
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync();

        return TypedResults.Ok(new PaginatedItems<AuctionItem>(pageIndex, pageSize, totalItems, itemsOnPage));
    }

    private static async Task<Results<Ok<List<AuctionItem>>, BadRequest<string>>> GetItemByIds(
        [AsParameters] AuctionServices services, int[] ids)
    {
        var items = await services.Context.AuctionItems.Where(auction => ids.Contains(auction.Id)).ToListAsync();

        return TypedResults.Ok(items);
    }

    private static async Task<Results<Ok<AuctionItem>, BadRequest<string>, NotFound>> GetItemById(
        [AsParameters] AuctionServices services, int id)
    {
        if (id <= 0)
        {
            return TypedResults.BadRequest("Id is not valid.");
        }

        var item = await services.Context.AuctionItems
            .Include(ci => ci.AuctionType)
            .SingleOrDefaultAsync(ci => ci.Id == id);

        if (item == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(item);
    }

    public static async Task<Results<Ok<PaginatedItems<AuctionItem>>, BadRequest<string>>> GetAllItems(
        [AsParameters] PaginationRequest request,
        [AsParameters] AuctionServices services)
    {
        var pageSize = request.PageSize;
        var pageIndex = request.PageIndex;

        var data = await services.Context.AuctionItems
            .OrderBy(auction => auction.Name)
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var count = services.Context.AuctionItems
            .Count();

        return TypedResults.Ok(new PaginatedItems<AuctionItem>(pageIndex, pageSize, count, data));
    }

    private static bool IsImageFile(IFormFile file)
    {
        var mimeType = file.ContentType.ToLowerInvariant();
        return mimeType == "image/png" ||
               mimeType == "image/gif" ||
               mimeType == "image/jpeg" ||
               mimeType == "image/bmp" ||
               mimeType == "image/tiff" ||
               mimeType == "image/wmf" ||
               mimeType == "image/jp2" ||
               mimeType == "image/svg+xml" ||
               mimeType == "image/webp";
    }
}