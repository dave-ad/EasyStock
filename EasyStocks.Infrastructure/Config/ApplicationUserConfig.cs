//namespace EasyStocks.Infrastructure.Config;

//internal class ApplicationUserConfig : IEntityTypeConfiguration<ApplicationUser>
//{
//    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
//    {
//        builder.ToTable("AspNetUSers");

//        // Configure additional ApplicationUser properties
//        builder.OwnsOne(x => x.Name, c =>
//        {
//            c.Property(x => x.Last).HasMaxLength(50).IsRequired();
//            c.Property(x => x.First).HasMaxLength(50).IsRequired();
//            c.Property(x => x.Others).HasMaxLength(50);
//        });

//        builder.OwnsOne(x => x.MobileNumber, y =>
//        {
//            y.Property(x => x.Value).HasMaxLength(11).IsRequired();
//            y.Property(x => x.Hash);
//        });

//        builder.Property(u => u.Gender).HasMaxLength(10);
//        // Additional configuration for other properties

//        // Configure Identity base properties (UserName, Email, etc.)
//        builder.Property(u => u.UserName).HasMaxLength(256).IsRequired();
//        builder.Property(u => u.NormalizedUserName).HasMaxLength(256).IsRequired();
//        builder.Property(u => u.Email).HasMaxLength(256).IsRequired();
//        builder.Property(u => u.NormalizedEmail).HasMaxLength(256).IsRequired();

//        // Indexes
//        builder.HasIndex(u => u.NormalizedUserName).IsUnique();
//        builder.HasIndex(u => u.NormalizedEmail).IsUnique();
//    }
//}
