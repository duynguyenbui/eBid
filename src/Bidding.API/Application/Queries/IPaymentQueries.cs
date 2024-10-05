namespace eBid.Bidding.API.Application.Queries;

public interface IPaymentQueries
{
    Task<IEnumerable<CardType>> GetCardTypesAsync();
}