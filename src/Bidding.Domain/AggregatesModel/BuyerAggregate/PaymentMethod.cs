using System.ComponentModel.DataAnnotations;

namespace eBid.Bidding.Domain.AggregatesModel.BuyerAggregate;

public class PaymentMethod : Entity
{
    [Required] private string _alias;
    [Required] private string _cardNumber;
    private string _securityNumber;
    [Required] private string _cardHolderName;
    private DateTime _expiration;

    private int _cardTypeId;
    public CardType CardType { get; private set; }

    protected PaymentMethod() { }

    public PaymentMethod(int cardTypeId, string alias, string cardNumber, string securityNumber, string cardHolderName,
        DateTime expiration)
    {
        _cardNumber = !string.IsNullOrWhiteSpace(cardNumber)
            ? cardNumber
            : throw new BiddingDomainException(nameof(cardNumber));
        _securityNumber = !string.IsNullOrWhiteSpace(securityNumber)
            ? securityNumber
            : throw new BiddingDomainException(nameof(securityNumber));
        _cardHolderName = !string.IsNullOrWhiteSpace(cardHolderName)
            ? cardHolderName
            : throw new BiddingDomainException(nameof(cardHolderName));

        if (expiration < DateTime.UtcNow)
        {
            throw new BiddingDomainException(nameof(expiration));
        }

        _alias = alias;
        _expiration = expiration;
        _cardTypeId = cardTypeId;
    }

    public bool IsEqualTo(int cardTypeId, string cardNumber, DateTime expiration)
    {
        return _cardTypeId == cardTypeId
               && _cardNumber == cardNumber
               && _expiration == expiration;
    }
}