namespace EasyStocks.Infrastructure.Config;

internal class CorporateBrokerConfig : IEntityTypeConfiguration<CorporateBroker>
{
    public void Configure(EntityTypeBuilder<CorporateBroker> builder)
    {
        builder.ToTable(nameof(CorporateBroker));
        builder.HasKey(x => x.CorporateBrokerId);

        builder.OwnsOne(x => x.CompanyName);

        builder.OwnsOne(x => x.CompanyEmail, y =>
        {
            y.Property(x => x.Value).HasMaxLength(100).IsRequired();
            y.Property(x => x.Hash);
        });

        builder.OwnsOne(x => x.CompanyMobileNumber, y =>
        {
            y.Property(x => x.Value).HasMaxLength(11).IsRequired();
            y.Property(x => x.Hash);
        });

        builder.OwnsOne(x => x.CorporateAddress, y =>
        {
            y.Property(z => z.StreetNo).HasMaxLength(100)
                .HasColumnName("Street_Number")
                .IsRequired();

            y.Property(z => z.StreetName).HasMaxLength(100)
                .HasColumnName("Street_Name")
                .IsRequired();

            y.Property(z => z.City).HasMaxLength(50)
                .HasColumnName("City")
                .IsRequired();

            y.Property(z => z.State).HasMaxLength(50)
                .HasColumnName("State")
                .IsRequired();

            y.Property(z => z.ZipCode).HasMaxLength(6)
                .HasColumnName("Street_Number")
                .IsRequired();
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
                .HasColumnName("City")
                .IsRequired();

            y.Property(z => z.State).HasMaxLength(50)
                .HasColumnName("State")
                .IsRequired();

            y.Property(z => z.ZipCode).HasMaxLength(6)
                .HasColumnName("Street_Number")
                .IsRequired();
        });

        builder.OwnsOne(x => x.CACRegistrationNumber, y =>
        {
            y.Property(x => x.Value).HasMaxLength(8).IsRequired();
            y.Property(x => x.Hash);
        });
        
        builder.OwnsOne(x => x.StockBrokerLicenseNumber, y =>
        {
            y.Property(x => x.Value).HasMaxLength(9).IsRequired();
            y.Property(x => x.Hash);
        });

        builder.OwnsOne(x => x.PositionInOrg);
    }
}