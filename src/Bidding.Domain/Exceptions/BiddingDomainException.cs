namespace eBid.Bidding.Domain.Exceptions;

/// <summary>
/// Exception type for domain exceptions
/// </summary>
public class BiddingDomainException : Exception
{
    public BiddingDomainException()
    {
    }

    public BiddingDomainException(string message)
        : base(message)
    {
    }

    public BiddingDomainException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}