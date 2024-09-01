﻿namespace EasyStocks.Infrastructure.Config;

internal class EasyStockUserConfig : IEntityTypeConfiguration<EasyStockUser>
{
    public void Configure(EntityTypeBuilder<EasyStockUser> builder)
    {
        builder.HasBaseType<User>();

        builder.Property(x => x.DateOfBirth);
        builder.OwnsOne(x => x.Address, y =>
        {
            y.Property(z => z.StreetNo).HasMaxLength(100);
            y.Property(z => z.StreetName).HasMaxLength(100);
            y.Property(z => z.City).HasMaxLength(50);
            y.Property(z => z.State).HasMaxLength(50);
            y.Property(z => z.ZipCode).HasMaxLength(10);
        });
        builder.OwnsOne(x => x.NIN, y =>
        {
            y.Property(z => z.Value).HasMaxLength(20).IsRequired();
        });

        builder.HasMany(u => u.Transactions)
               .WithOne(t => t.User)
               .HasForeignKey(t => t.UserId);
    }
}