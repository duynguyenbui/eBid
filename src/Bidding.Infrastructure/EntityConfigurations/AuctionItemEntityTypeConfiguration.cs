namespace eBid.Bidding.Infrastructure.EntityConfigurations;

public class AuctionItemEntityTypeConfiguration : IEntityTypeConfiguration<AuctionItem>
{
    public void Configure(EntityTypeBuilder<AuctionItem> auctionItemEntityTypeBuilder)
    {
        auctionItemEntityTypeBuilder.ToTable("auctionitems");

        auctionItemEntityTypeBuilder.HasKey(ai => ai.Id);

        auctionItemEntityTypeBuilder.Property(ai => ai.AuctionEnd)
            .IsRequired();

        auctionItemEntityTypeBuilder.Property(ai => ai.Seller)
            .IsRequired()
            .HasMaxLength(200);

        auctionItemEntityTypeBuilder.Property(ai => ai.StartingPrice)
            .IsRequired();
    }
}