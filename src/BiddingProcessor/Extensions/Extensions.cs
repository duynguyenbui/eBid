
namespace eBid.BiddingProcessor.Extensions;

public static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.AddNpgsqlDataSource("biddingdb");

        builder.Services.AddHostedService<Worker>();
    }
}