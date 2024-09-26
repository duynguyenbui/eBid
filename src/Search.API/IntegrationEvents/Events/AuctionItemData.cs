namespace eBid.Search.API.IntegrationEvents.Events;

public record AuctionItemData(
    int Id,
    string Name,
    string Description,
    string? PictureUrl,
    string SellerId,
    string? WinnerId,
    decimal? SoldAmount,
    decimal StartingPrice,
    bool OnSell,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    DateTime EndingTime,
    string Status,
    int AuctionTypeId,
    string AuctionType);