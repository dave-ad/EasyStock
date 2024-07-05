namespace EasyStocks.Infrastructure.Config;

internal class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(nameof(User));
        builder.HasKey(x => x.UserId);

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

        // Configure relationship to Broker
        builder.HasOne(u => u.Broker) // User has one Broker
               .WithMany(b => b.Users) // Broker has many Users
               .HasForeignKey(u => u.BrokerId) // User.BrokerId is the foreign key
               .IsRequired(); // Relationship is required
    }
}