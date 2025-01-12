namespace EasyStocks.Infrastructure.Config;

internal class BrokerConfig : IEntityTypeConfiguration<Broker>
{
    public void Configure(EntityTypeBuilder<Broker> builder)
    {
        builder.ToTable(nameof(Broker));

        // Configuring inherited properties from User
        builder.Property(x => x.Email).HasMaxLength(256).IsRequired();
        builder.Property(x => x.PhoneNumber).HasMaxLength(15);
        builder.Property(x => x.Gender).HasMaxLength(10);


        builder.OwnsOne(x => x.Name, y =>
        {
            y.Property(z => z.First).HasMaxLength(50).IsRequired();
            y.Property(z => z.Last).HasMaxLength(50).IsRequired();
            y.Property(z => z.Others).HasMaxLength(100);
        });

        builder.OwnsOne(x => x.Address, y =>
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
                .HasColumnName("ZipCode")
                .IsRequired();
        });

        builder.OwnsOne(x => x.NIN, y =>
        {
            y.Property(z => z.Value).HasMaxLength(20).IsRequired();
            y.Property(z => z.Hash);
        });

        builder.OwnsOne(x => x.BrokerLicense, y =>
        {
            y.Property(z => z.Value).HasMaxLength(9).IsRequired();
            y.Property(z => z.Hash);
        });

        builder.Property(x => x.DateCertified).HasColumnType("date");
        builder.Property(x => x.ProfessionalQualification).HasMaxLength(200);
        builder.Property(x => x.Status).IsRequired();
        builder.Property(x => x.BrokerType).IsRequired();
    }
}
