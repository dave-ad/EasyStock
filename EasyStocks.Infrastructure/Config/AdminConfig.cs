namespace EasyStocks.Infrastructure.Config;

internal class AdminConfig : IEntityTypeConfiguration<Admin>
{
    public void Configure(EntityTypeBuilder<Admin> builder)
    {
        //builder.ToTable(nameof(Admin));
        //builder.HasKey(x => x.Id);

        builder.HasBaseType<User>();

        // Configure properties specific to Admin
        builder.Property(x => x.SuperAdminLevel);
        builder.Property(x => x.Permissions);
    }
}