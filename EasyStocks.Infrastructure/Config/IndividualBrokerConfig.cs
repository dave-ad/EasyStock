namespace EasyStocks.Infrastructure.Config;

internal class IndividualBrokerConfig : IEntityTypeConfiguration<IndividualBroker>
{
    public void Configure(EntityTypeBuilder<IndividualBroker> builder)
    {
        builder.ToTable(nameof(IndividualBroker));
        builder.HasKey(x => x.IndividualBrokerId);

        builder.OwnsOne(x => x.BusinessAddress, y =>
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

        builder.OwnsOne(x => x.StockBrokerLicenseNumber, y =>
        {
            y.Property(x => x.Value).HasMaxLength(100).IsRequired();
            y.Property(x => x.Hash);
        });

        builder.OwnsOne(x => x.ProfessionalQualification);

    }
}