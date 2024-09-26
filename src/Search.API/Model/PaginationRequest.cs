namespace eBid.Search.API.Model;

public record PaginationRequest(int From = 0, int Size = 10);