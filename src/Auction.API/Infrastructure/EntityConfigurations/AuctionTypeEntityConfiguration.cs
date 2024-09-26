namespace eBid.Auction.API.Infrastructure.EntityConfigurations;

public class AuctionTypeEntityConfiguration : IEntityTypeConfiguration<AuctionType>
{
    public void Configure(EntityTypeBuilder<AuctionType> builder)
    {
        builder.ToTable("AuctionType");

        builder.Property(at => at.Type)
            .HasMaxLength(100);
    }
}