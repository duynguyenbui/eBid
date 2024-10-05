namespace eBid.Bidding.API.Application.Queries;

public class PaymentQueries(BiddingContext context) : IPaymentQueries
{
    public async Task<IEnumerable<CardType>> GetCardTypesAsync() => 
        await context.CardTypes.Select(ct => new CardType() { Id = ct.Id, Name = ct.Name }).ToListAsync();
}