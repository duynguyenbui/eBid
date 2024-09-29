namespace eBid.Auction.API.Infrastructure;

public partial class AuctionContextSeed(
    IWebHostEnvironment env,
    IOptions<AuctionOptions> settings,
    ILogger<AuctionContextSeed> logger,
    IAuctionAI auctionAi)
    : IDbSeeder<AuctionContext>
{
    public async Task SeedAsync(AuctionContext context)
    {
#if DEBUG
        var useCustomizationData = settings.Value.UseCustomizationData;
        var contentRootPath = env.ContentRootPath;
        var picturePath = env.WebRootPath;

        // Workaround from https://github.com/npgsql/efcore.pg/issues/292#issuecomment-388608426
        context.Database.OpenConnection();
        ((NpgsqlConnection)context.Database.GetDbConnection()).ReloadTypes();

        var auctionTypes = new[]
        {
            new AuctionType { Type = "English" }, new AuctionType { Type = "Dutch" },
            new AuctionType { Type = "Sealed-bid" }, new AuctionType { Type = "Vickrey" },
            new AuctionType { Type = "Reverse" }, new AuctionType { Type = "Double" },
            new AuctionType { Type = "Combinatorial" }, new AuctionType { Type = "Buyout" },
            new AuctionType { Type = "Japanese" }, new AuctionType { Type = "Multi-unit" },
            new AuctionType { Type = "Online" }, new AuctionType { Type = "Silent" },
            new AuctionType { Type = "Yankee" }, new AuctionType { Type = "All-pay" },
            new AuctionType { Type = "Penny" }, new AuctionType { Type = "Reserve price" },
            new AuctionType { Type = "Bid" }, new AuctionType { Type = "Lowest unique bid" },
            new AuctionType { Type = "Other" }
        };

        if (!context.AuctionTypes.Any())
        {
            await context.AuctionTypes.AddRangeAsync(auctionTypes);
            await context.SaveChangesAsync();
            logger.LogInformation($"Seeded {auctionTypes.Length} of auctions types");
        }

        if (!context.AuctionItems.Any() && false)
        {
            var sourcePath = Path.Combine(contentRootPath, "Setup", "auctions.json");
            var sourceJson = File.ReadAllText(sourcePath);
            var sourceItems = JsonSerializer.Deserialize<AuctionSourceEntry[]>(sourceJson);

            context.AuctionTypes.RemoveRange(context.AuctionTypes);
            await context.AuctionTypes.AddRangeAsync(
                sourceItems.Select(x => x.AuctionType).Distinct()
                    .Select(auctionType => new AuctionType() { Type = auctionType })
            );

            await context.SaveChangesAsync();

            logger.LogInformation("Seeded auctions with {NumTypes} types", context.AuctionTypes.Count());


            var typeIdsByName = await context.AuctionTypes.ToDictionaryAsync(x => x.Type, x => x.Id);

            var auctionItems = sourceItems.Select(source => new AuctionItem()
            {
                Name = source.Name,
                Description = source.Description,
                SellerId = source.SellerId,
                StartingPrice = source.StartingPrice,
                AuctionTypeId = typeIdsByName[source.AuctionType],
                CreatedAt = source.CreatedAt,
                UpdatedAt = source.UpdatedAt,
                EndingTime = source.EndingTime,
                Status = (AuctionStatus)Enum.Parse(typeof(AuctionStatus), source.Status),
                PictureUrl = source.PictureUrl,
                PicturePublicId = source.PicturePublicId
            }).ToArray();

            if (auctionAi.IsEnabled)
            {
                logger.LogInformation("Generating {NumItems} embeddings", auctionItems.Length);

                var embeddings = await auctionAi.GetEmbeddingsAsync(auctionItems);
                for (int i = 0; i < embeddings.Count; i++)
                {
                    auctionItems[i].Embedding = embeddings[i];
                }
            }

            await context.AuctionItems.AddRangeAsync(auctionItems);
            await context.SaveChangesAsync();

            logger.LogInformation("Seeded auctions with {NumItems} items", context.AuctionItems.Count());
        }
#else
        logger.LogInformation("Skipping seeding of AuctionContext");
#endif
    }

    private class AuctionSourceEntry
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string? SellerId { get; set; }
        public decimal StartingPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime EndingTime { get; set; }
        public string? PicturePublicId { get; set; }
        public string? PictureUrl { get; set; }
        public string Status { get; set; }
        public string AuctionType { get; set; }
    }
}