namespace EasyStocks.Infrastructure.Config;

internal class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(nameof(User));
        builder.HasKey(x => x.Id);

        // Configure Identity-related properties
        builder.Property(x => x.Email).HasMaxLength(256).IsRequired();
        builder.Property(x => x.UserName).HasMaxLength(256);
        builder.Property(x => x.NormalizedUserName).HasMaxLength(256);
        builder.Property(x => x.NormalizedEmail).HasMaxLength(256);
        builder.Property(x => x.NormalizedEmail).HasMaxLength(256);
        builder.Property(x => x.PasswordHash);
        builder.Property(x => x.SecurityStamp);
        builder.Property(x => x.ConcurrencyStamp);
        builder.Property(x => x.PhoneNumber);
        builder.Property(x => x.PhoneNumberConfirmed);
        builder.Property(x => x.TwoFactorEnabled);
        builder.Property(x => x.LockoutEnd);
        builder.Property(x => x.LockoutEnabled);
        builder.Property(x => x.AccessFailedCount);


        // value object configuration
        builder.OwnsOne(x => x.Name, y =>
        {
            y.Property(z => z.Last).HasMaxLength(50).IsRequired();
            y.Property(z => z.First).HasMaxLength(50).IsRequired();
            y.Property(z => z.Others).HasMaxLength(50);
        });

        builder.OwnsOne(x => x.MobileNumber, y =>
        {
            y.Property(z => z.Value).HasMaxLength(11).IsRequired();
            y.Property(z => z.Hash);
        });

        builder.Property(x => x.Gender).HasMaxLength(10);
        builder.Property(x => x.PositionInOrg);
        builder.Property(x => x.DateOfEmployment);

        // Configure relationship to Broker
        builder.HasOne(u => u.Broker) // User has one Broker
               .WithMany(b => b.Users) // Broker has many Users
               .HasForeignKey(u => u.BrokerId) // User.BrokerId is the foreign key
               .IsRequired(); // Relationship is required
    }
}