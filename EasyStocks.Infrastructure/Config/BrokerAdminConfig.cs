namespace EasyStocks.Infrastructure.Config;

internal class BrokerAdminConfig : IEntityTypeConfiguration<BrokerAdmin>
{
    public void Configure(EntityTypeBuilder<BrokerAdmin> builder)
    {
        builder.HasBaseType<User>();
        builder.Property(x => x.PositionInOrg);
        builder.Property(x => x.DateOfEmployment);
        builder.Property(x => x.Status);

        builder.HasOne(u => u.Broker)
               .WithMany(b => b.Users)
               .HasForeignKey(u => u.BrokerId)
               .IsRequired();
    }
}