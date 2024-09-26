internal static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddHttpForwarderWithServiceDiscovery();

        builder.Services.AddHealthChecks()
            .AddUrlGroup(new Uri("http://auction-api/health"), name: "auctionapi-check")
            .AddUrlGroup(new Uri("http://identity-api/health"), name: "identityapi-check");
    }
}