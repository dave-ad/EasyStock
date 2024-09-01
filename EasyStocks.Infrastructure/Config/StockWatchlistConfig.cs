namespace EasyStocks.Infrastructure.Config;

internal class StockWatchListConfig : IEntityTypeConfiguration<StockWatchList>
{
    public void Configure(EntityTypeBuilder<StockWatchList> builder)
    {
        builder.ToTable(nameof(StockWatchList));
        builder.HasKey(x => x.WatchlistId);

        builder.HasOne(w => w.User)
            .WithMany(u => u.Watchlists)
            .HasForeignKey(w => w.UserId);

        builder.HasOne(w => w.Stock)
            .WithMany(s => s.Watchlists)
            .HasForeignKey(w => w.StockId);
    }
}