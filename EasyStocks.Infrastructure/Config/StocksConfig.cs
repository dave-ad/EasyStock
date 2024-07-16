
namespace EasyStocks.Infrastructure.Config;

internal class StocksConfig : IEntityTypeConfiguration<Stocks>
{
    public void Configure(EntityTypeBuilder<Stocks> builder)
    {
        builder.ToTable(nameof(Stocks));
        builder.HasKey(x => x.Id);

        builder.Property(s => s.PricePerUnit)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

        builder.Property(s => s.InitialDeposit)
                .IsRequired()
                .HasColumnType("decimal(18,2)");
    }
}