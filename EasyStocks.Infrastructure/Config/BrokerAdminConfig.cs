namespace EasyStocks.Infrastructure.Config;

internal class BrokerAdminConfig : IEntityTypeConfiguration<BrokerAdmin>
{
    public void Configure(EntityTypeBuilder<BrokerAdmin> builder)
    {
        //builder.ToTable(nameof(BrokerAdmin));
        //builder.HasKey(x => x.Id);

        builder.HasBaseType<User>();

        // Configure properties specific to BrokerAdmin
        builder.Property(x => x.PositionInOrg);
        builder.Property(x => x.DateOfEmployment);
        builder.Property(x => x.Status);

        // Configure relationship to Broker
        builder.HasOne(u => u.Broker)
               .WithMany(b => b.Users)
               .HasForeignKey(u => u.BrokerId)
               .IsRequired();
    }
}