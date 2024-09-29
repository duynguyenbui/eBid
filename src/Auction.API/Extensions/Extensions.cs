namespace eBid.Auction.API.Extensions;

public static class Extensions
{
    /// <summary>
    /// Adds the application services to the host builder.
    /// 
    /// This method configures the NpgsqlDbContext for the AuctionContext, sets up the
    /// migration for the AuctionContext with the AuctionContextSeed, adds the integration
    /// services that consume the DbContext, sets up the RabbitMqEventBus, binds the
    /// AuctionOptions and OpenAIOptions to the configuration, and adds the OpenAI client
    /// singleton if an API key is provided in the configuration. Finally, it adds the
    /// AuctionAI singleton.
    /// 
    /// </summary>
    /// <param name="builder">The host application builder.</param>
    /// <returns>The modified host application builder.</returns>
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.AddDefaultAuthentication();

        builder.AddNpgsqlDbContext<AuctionContext>("auctiondb", configureDbContextOptions: dbContextOptionsBuilder =>
        {
            dbContextOptionsBuilder.UseNpgsql(npgsqlDbContextOptionsBuilder =>
            {
                npgsqlDbContextOptionsBuilder.UseVector();
            });
        });

        builder.Services.AddMigration<AuctionContext, AuctionContextSeed>();

        builder.Services.AddHttpContextAccessor();
        // Add the integration services that consume the DbContext
        builder.Services.AddTransient<IIntegrationEventLogService, IntegrationEventLogService<AuctionContext>>();
        builder.Services.AddTransient<IAuctionIntegrationEventService, AuctionIntegrationEventService>();
        builder.Services.AddTransient<IIdentityService, IdentityService>();

        builder.AddRabbitMqEventBus("eventbus");

        builder.Services.AddOptions<AuctionOptions>()
            .BindConfiguration(nameof(AuctionOptions));

        builder.Services.AddOptions<OpenAIOptions>()
            .BindConfiguration(nameof(OpenAIOptions));

        builder.Services.AddOptions<CloudinaryOptions>()
            .BindConfiguration(nameof(CloudinaryOptions));

        using var serviceProvider = builder.Services.BuildServiceProvider();
        var openAIOptions = serviceProvider.GetRequiredService<IOptions<OpenAIOptions>>().Value;

        if (!string.IsNullOrWhiteSpace(builder.Configuration["OpenAIOptions:ApiKey"]))
        {
            builder.Services.AddSingleton(new OpenAIClient(new OpenAIAuthentication(openAIOptions.ApiKey),
                new OpenAIClientSettings(domain: openAIOptions.Endpoint)));
        }

        builder.Services.AddSingleton<IAuctionAI, AuctionAI>();

        // Specifically register the image service for cloudinary
        builder.Services.AddScoped<IImageService<ImageUploadResult, DeletionResult>, ImageService>();
    }
}