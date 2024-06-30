namespace EasyStocks.Infrastructure;

public interface IEasyStockAppDbContext
{
    DbSet<CorporateBroker> CorporateBrokers { get; set; }
    DbSet<IndividualBroker> IndividualBrokers { get; set; }
    DbSet<FreelanceBroker> FreelanceBrokers { get; set; }
}