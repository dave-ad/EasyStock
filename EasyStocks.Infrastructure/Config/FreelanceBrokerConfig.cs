namespace EasyStocks.Infrastructure.Config;

internal class FreelanceBrokerConfig : IEntityTypeConfiguration<Broker>
{
    public void Configure(EntityTypeBuilder<Broker> builder)
    {
        builder.ToTable(nameof(Broker));
        builder.HasKey(x => x.BrokerId);

        //builder.OwnsOne(x => x.Name, y =>
        //{
        //    y.Property(z => z.Last).HasMaxLength(50).IsRequired();
        //    y.Property(z => z.First).HasMaxLength(50).IsRequired();
        //    y.Property(z => z.Others).HasMaxLength(50);
        //});

        //builder.OwnsOne(x => x.Email, y =>
        //{
        //    y.Property(z => z.Value).HasMaxLength(100).IsRequired();
        //    y.Property(z => z.Hash);
        //});

        //builder.OwnsOne(x => x.MobileNumber, y =>
        //{
        //    y.Property(z => z.Value).HasMaxLength(11).IsRequired();
        //    y.Property(z => z.Hash);
        //});



        builder.OwnsOne(x => x.StockBrokerLicense, y =>
        {
            y.Property(z => z.Value).HasMaxLength(9).IsRequired();
            y.Property(z => z.Hash);
        });

        // Configure User navigation property
        builder.HasMany(b => b.Users)
             .WithOne(u => u.Broker) // Assuming User has a navigation property back to Broker
             .HasForeignKey(u => u.BrokerId) // Assuming User has a property named BrokerId
             .IsRequired();
    }
}
