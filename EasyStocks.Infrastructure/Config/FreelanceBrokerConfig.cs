namespace EasyStocks.Infrastructure.Config;

internal class FreelanceBrokerConfig : IEntityTypeConfiguration<Broker>
{
    public void Configure(EntityTypeBuilder<Broker> builder)
    {
        builder.ToTable(nameof(Broker));
        builder.HasKey(x => x.BrokerId);

        builder.OwnsOne(x => x.StockBrokerLicense, y =>
        {
            y.Property(z => z.Value).HasMaxLength(9).IsRequired();
            y.Property(z => z.Hash);
        });

        // Configure User navigation property
        builder.HasMany(b => b.Users)
             .WithOne(u => u.Broker)
             .HasForeignKey(u => u.BrokerId)
             .IsRequired();
    }
}
