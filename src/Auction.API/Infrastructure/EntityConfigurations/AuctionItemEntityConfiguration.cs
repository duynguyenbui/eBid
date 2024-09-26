namespace eBid.Auction.API.Infrastructure.EntityConfigurations;

public class AuctionItemEntityConfiguration : IEntityTypeConfiguration<AuctionItem>
{
    public void Configure(EntityTypeBuilder<AuctionItem> builder)
    {
        builder.ToTable("Auction");

        builder.HasKey(ai => ai.Id);
        builder.Property(ai => ai.Id)
            .ValueGeneratedOnAdd();

        builder.Property(ai => ai.Name)
            .HasMaxLength(50);

        builder.Property(ci => ci.Embedding)
            .HasColumnType("vector(768)");

        builder.HasOne(ai => ai.AuctionType)
            .WithMany();

        builder.HasIndex(ai => ai.Name);
    }
}