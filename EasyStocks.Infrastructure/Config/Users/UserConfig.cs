namespace EasyStocks.Infrastructure.Config;

internal class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(nameof(User));
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.Email).HasMaxLength(256).IsRequired();
        builder.Property(x => x.UserName).HasMaxLength(256);
        builder.Property(x => x.NormalizedUserName).HasMaxLength(256);
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

        builder.Property(x => x.Gender).HasMaxLength(10);

        //Configure the discriminator column for inheritance
        builder.HasDiscriminator<string>("Discriminator")
            .HasValue<User>("User")
            .HasValue<Admin>("Admin")
            .HasValue<Broker>("Broker")
            .HasValue<AppUser>("AppUser");
    }
}