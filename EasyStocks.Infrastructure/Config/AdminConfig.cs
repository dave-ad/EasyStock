namespace EasyStocks.Infrastructure.Config;

internal class AdminConfig : IEntityTypeConfiguration<Admin>
{
    public void Configure(EntityTypeBuilder<Admin> builder)
    {
        builder.HasBaseType<User>();
        builder.Property(x => x.SuperAdminLevel);
        builder.Property(x => x.Permissions);
    }
}