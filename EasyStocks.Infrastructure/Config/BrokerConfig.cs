namespace EasyStocks.Infrastructure.Config;

internal class BrokerConfig : IEntityTypeConfiguration<Broker>
{
    public void Configure(EntityTypeBuilder<Broker> builder)
    {
        builder.ToTable(nameof(Broker));
        builder.HasKey(x => x.BrokerId);

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
                .HasColumnName("Street_Number")
                .IsRequired();

            y.Property(z => z.StreetName).HasMaxLength(100)
                .HasColumnName("Street_Name")
                .IsRequired();

            y.Property(z => z.City).HasMaxLength(50)
                .IsRequired();

            y.Property(z => z.State).HasMaxLength(50)
                .IsRequired();

            y.Property(z => z.ZipCode).HasMaxLength(6)
                .IsRequired();
        });

        builder.OwnsOne(x => x.CACRegistrationNumber, y =>
        {
            y.Property(z => z.Value).HasMaxLength(8).IsRequired();
            y.Property(z => z.Hash);
        });

        builder.OwnsOne(x => x.StockBrokerLicenseNumber, y =>
        {
            y.Property(z => z.Value).HasMaxLength(9).IsRequired();
            y.Property(z => z.Hash);
        });

        builder.OwnsOne(x => x.BusinessAddress, y =>
        {
            y.Property(z => z.StreetNo).HasMaxLength(100)
                .HasColumnName("Street_Number")
                .IsRequired();

            y.Property(z => z.StreetName).HasMaxLength(100)
                .HasColumnName("Street_Name")
                .IsRequired();

            y.Property(z => z.City).HasMaxLength(50)
                .IsRequired();

            y.Property(z => z.State).HasMaxLength(50)
                .IsRequired();

            y.Property(z => z.ZipCode).HasMaxLength(6)
                .IsRequired();
        });
    }
}