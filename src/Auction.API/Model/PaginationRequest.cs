namespace eBid.Auction.API.Model;

public record PaginationRequest(int PageSize = 10, int PageIndex = 0);