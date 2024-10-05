using CardType = eBid.Bidding.Domain.AggregatesModel.BuyerAggregate.CardType;

namespace eBid.Bidding.API.Infrastructure;

public class BiddingContextSeed : IDbSeeder<BiddingContext>
{
    public async Task SeedAsync(BiddingContext context)
    {
        if (!context.CardTypes.Any())
        {
            context.CardTypes.AddRange(GetPredefinedCardTypes());

            await context.SaveChangesAsync();
        }

        await context.SaveChangesAsync();
    }

    private static IEnumerable<CardType> GetPredefinedCardTypes()
    {
        return Enumeration.GetAll<CardType>();
    }
}