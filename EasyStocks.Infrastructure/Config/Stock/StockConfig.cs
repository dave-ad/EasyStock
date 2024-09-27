
namespace EasyStocks.Infrastructure.Config;

internal class StockConfig : IEntityTypeConfiguration<Stock>
{
    public void Configure(EntityTypeBuilder<Stock> builder)
    {
        builder.ToTable(nameof(Stock));
        builder.HasKey(x => x.StockId);

        builder.Property(s => s.TickerSymbol)
                      .IsRequired()
                      .HasMaxLength(10); // Adjust length as needed

        builder.Property(s => s.CompanyName)
               .IsRequired()
               .HasMaxLength(100); // Adjust length as needed

        builder.Property(s => s.Exchange)
               .IsRequired()
               .HasMaxLength(50); // Adjust length as needed

        // Prices
        builder.Property(s => s.OpeningPrice)
               .IsRequired()
               .HasColumnType("decimal(18,2)");

        builder.Property(s => s.ClosingPrice)
               .IsRequired()
               .HasColumnType("decimal(18,2)");

        builder.Property(s => s.CurrentPrice)
               .IsRequired()
               .HasColumnType("decimal(18,2)");

        builder.Property(s => s.DayHigh)
               .IsRequired()
               .HasColumnType("decimal(18,2)");

        builder.Property(s => s.DayLow)
               .IsRequired()
               .HasColumnType("decimal(18,2)");

        builder.Property(s => s.YearHigh)
               .IsRequired()
               .HasColumnType("decimal(18,2)");

        builder.Property(s => s.YearLow)
               .IsRequired()
               .HasColumnType("decimal(18,2)");

        // Financial Metrics
        builder.Property(s => s.OutstandingShares)
               .IsRequired();

        builder.Property(s => s.DividendYield)
               .HasColumnType("decimal(5,2)"); // Adjust precision as needed

        builder.Property(s => s.EarningsPerShare)
               .IsRequired()
               .HasColumnType("decimal(18,2)");

        builder.Property(s => s.Volume)
               .IsRequired();

        builder.Property(s => s.Beta)
               .HasColumnType("decimal(5,2)"); // Adjust precision as needed

        // Computed Properties
        builder.Ignore(s => s.MarketCapitalization);
        builder.Ignore(s => s.PriceEarningsRatio);
    }
}