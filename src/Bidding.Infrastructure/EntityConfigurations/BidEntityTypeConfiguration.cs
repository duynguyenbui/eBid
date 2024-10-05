namespace eBid.Bidding.Infrastructure.EntityConfigurations;

public class BidEntityTypeConfiguration : IEntityTypeConfiguration<Bid>
{
    public void Configure(EntityTypeBuilder<Bid> bidEntityConfigurationTypeBuilder)
    {
        bidEntityConfigurationTypeBuilder.ToTable("bids");

        bidEntityConfigurationTypeBuilder.Property(bi => bi.AuctionItemId)
            .IsRequired()
            .HasMaxLength(200);

        bidEntityConfigurationTypeBuilder.Property(bi => bi.Bidder)
            .IsRequired()
            .HasMaxLength(200);

        bidEntityConfigurationTypeBuilder.Property(bi => bi.Amount)
            .IsRequired();

        bidEntityConfigurationTypeBuilder
            .HasOne(bid => bid.AuctionItem)
            .WithMany()
            .HasForeignKey(bid => bid.AuctionItemId);
    }
}