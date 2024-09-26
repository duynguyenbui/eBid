namespace eBid.Auction.API.Infrastructure;

/// <remarks>
/// Add migrations using the following command inside the 'Auction.API' project directory:
///
/// dotnet ef migrations add --context AuctionContext [migration-name]
public class AuctionContext(DbContextOptions<AuctionContext> options) : DbContext(options)
{
    public DbSet<AuctionItem> AuctionItems { get; set; }
    public DbSet<AuctionType> AuctionTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("vector");

        modelBuilder.ApplyConfiguration(new AuctionItemEntityConfiguration());
        modelBuilder.ApplyConfiguration(new AuctionTypeEntityConfiguration());

        // Add the outbox table to this context
        modelBuilder.UseIntegrationEventLogs();
    }
}