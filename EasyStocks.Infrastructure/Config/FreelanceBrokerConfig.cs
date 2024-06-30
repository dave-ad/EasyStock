namespace EasyStocks.Infrastructure.Config;

internal class FreelanceBrokerConfig : IEntityTypeConfiguration<FreelanceBroker>
{
    public void Configure(EntityTypeBuilder<FreelanceBroker> builder)
    {
        builder.ToTable(nameof(FreelanceBroker));
        builder.HasKey(x => x.FreelanceBrokerid);

        builder.OwnsOne(x => x.ProfessionalQualification);
    }
}