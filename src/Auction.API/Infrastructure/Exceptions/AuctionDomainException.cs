namespace eBid.Auction.API.Infrastructure.Exceptions;

/// <summary>
/// Exception type for app exceptions
/// </summary>
public class AuctionDomainException : Exception
{
    public AuctionDomainException()
    {
    }

    public AuctionDomainException(string message) : base(message)
    {
    }

    public AuctionDomainException(string message, Exception innerException) : base(message, innerException)
    {
    }
}