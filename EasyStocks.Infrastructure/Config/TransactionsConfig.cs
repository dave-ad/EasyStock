
namespace EasyStocks.Infrastructure.Config;

internal class TransactionsConfig : IEntityTypeConfiguration<Transactions>
{
    public void Configure(EntityTypeBuilder<Transactions> builder)
    {
        builder.ToTable(nameof(Transactions));
        builder.HasKey(x => x.TransactionId);

        builder.Property(x => x.Id).IsRequired(); ;
        builder.Property(x => x.StockId).IsRequired(); ;
        builder.Property(t => t.TransactionAmount)
       .HasColumnType("decimal(18,2)")
       .IsRequired(); ;

        builder.Property(t => t.TransactionDate)
               .IsRequired();

        builder.Property(t => t.Type)
               .IsRequired();

        builder.Property(t => t.Status)
               .IsRequired();

        // Configure relationships
        builder.HasOne(t => t.User)
               .WithMany(u => u.Transactions)
               .HasForeignKey(t => t.Id)
               .IsRequired();

        builder.HasOne(t => t.Stock)
               .WithMany()
               .HasForeignKey(t => t.StockId)
               .IsRequired();
    }
}