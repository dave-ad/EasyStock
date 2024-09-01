namespace EasyStocks.Infrastructure.Config;

internal class TransactionConfig : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable(nameof(Transaction));
        builder.HasKey(x => x.TransactionId);

        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.StockId).IsRequired();

        builder.Property(t => t.PricePerUnit)
               .IsRequired()
               .HasColumnType("decimal(18,2)");

        builder.Property(t => t.UnitPurchase)
               .IsRequired()
               .HasMaxLength(50); // Adjust length as needed

        builder.Property(t => t.TransactionAmount)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(t => t.TransactionDate)
               .IsRequired();

        builder.Property(t => t.Status)
               .IsRequired();

        // Configure relationships
        builder.HasOne(t => t.User)
               .WithMany(u => u.Transactions)
               .HasForeignKey(t => t.UserId)
               .IsRequired();

        builder.HasOne(t => t.Stock)
               .WithMany(s => s.Transactions)
               .HasForeignKey(t => t.StockId)
               .IsRequired();
    }
}