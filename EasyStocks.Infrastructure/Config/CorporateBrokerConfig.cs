namespace EasyStocks.Infrastructure.Config;

internal class CorporateBrokerConfig : IEntityTypeConfiguration<Broker>
{
    public void Configure(EntityTypeBuilder<Broker> builder)
    {
        builder.ToTable(nameof(Broker));
        builder.HasKey(x => x.BrokerId);

        builder.OwnsOne(x => x.CompanyEmail, y =>
        {
            y.Property(z => z.Value).HasMaxLength(100).IsRequired();
            y.Property(z => z.Hash);
        });

        builder.OwnsOne(x => x.CompanyMobileNumber, y =>
        {
            y.Property(z => z.Value).HasMaxLength(11).IsRequired();
            y.Property(z => z.Hash);
        });

        builder.OwnsOne(x => x.CompanyAddress, y =>
        {
            y.Property(z => z.StreetNo).HasMaxLength(100)
                .HasColumnName("Company_Street_Number")
                .IsRequired();

            y.Property(z => z.StreetName).HasMaxLength(100)
                .HasColumnName("Company_Street_Name")
                .IsRequired();

            y.Property(z => z.City).HasMaxLength(50)
                .HasColumnName("Company_City")
                .IsRequired();

            y.Property(z => z.State).HasMaxLength(50)
                .HasColumnName("Company_State")
                .IsRequired();

            y.Property(z => z.ZipCode).HasMaxLength(6)
                .HasColumnName("Company_ZipCode")
                .IsRequired();
        });

        builder.OwnsOne(x => x.CACRegistrationNumber, y =>
        {
            y.Property(z => z.Value).HasMaxLength(8).IsRequired();
            y.Property(z => z.Hash);
        });

        builder.OwnsOne(x => x.StockBrokerLicense, y =>
        {
            y.Property(z => z.Value).HasMaxLength(9).IsRequired();
            y.Property(z => z.Hash);
        });

        builder.OwnsOne(x => x.Name, y =>
        {
            y.Property(z => z.Last).HasMaxLength(50).IsRequired();
            y.Property(z => z.First).HasMaxLength(50).IsRequired();
            y.Property(z => z.Others).HasMaxLength(50);
        });

        builder.OwnsOne(x => x.Email, y =>
        {
            y.Property(z => z.Value).HasMaxLength(100).IsRequired();
            y.Property(z => z.Hash);
        });

        builder.OwnsOne(x => x.MobileNumber, y =>
        {
            y.Property(z => z.Value).HasMaxLength(11).IsRequired();
            y.Property(z => z.Hash);
        });

        // Configure User navigation property
        builder.HasMany(b => b.Users)
             .WithOne(u => u.Broker) // Assuming User has a navigation property back to Broker
             .HasForeignKey(u => u.BrokerId) // Assuming User has a property named BrokerId
             .IsRequired();
    }
}
