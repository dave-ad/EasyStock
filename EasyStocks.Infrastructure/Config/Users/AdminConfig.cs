namespace EasyStocks.Infrastructure.Config;

internal class AdminConfig : IEntityTypeConfiguration<Admin>
{
    public void Configure(EntityTypeBuilder<Admin> builder)
    {
        builder.ToTable(nameof(Admin));

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
    }
}