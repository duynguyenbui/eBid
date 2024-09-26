namespace eBid.Auction.API.Model;

public class AuctionItem
{
    public int Id { get; set; }

    [Required] public string Name { get; set; }

    public string Description { get; set; }

    public string? PictureUrl { get; set; }
    public string? PicturePublicId { get; set; }

    [Required] public string SellerId { get; set; }

    [AllowNull] public string? WinnerId { get; set; }

    public decimal? SoldAmount { get; set; } = null;

    public decimal StartingPrice { get; set; }

    public bool OnSell { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [Required] public DateTime EndingTime { get; set; }

    // Enumeration Status, including live, finished, and reserve not met 
    public AuctionStatus Status { get; set; } = AuctionStatus.Live;

    public int AuctionTypeId { get; set; }
    public AuctionType AuctionType { get; set; }

    /// <summary>Optional embedding for the catalog item's description.</summary>
    [JsonIgnore]
    public Vector? Embedding { get; set; }

    /// <summary>
    /// Determines if the auction item is allowed to be updated.
    /// </summary>
    public bool IsAllowedToUpdate() => Status == AuctionStatus.Live && DateTime.UtcNow < EndingTime && !OnSell;

    public AuctionItemData ToAuctionItemData()
    {
        return new AuctionItemData(Id, Name, Description, PictureUrl, SellerId, WinnerId, SoldAmount,
            StartingPrice, OnSell, CreatedAt, UpdatedAt, EndingTime, Status.ToString(), AuctionTypeId,
            AuctionType.Type);
    }
}