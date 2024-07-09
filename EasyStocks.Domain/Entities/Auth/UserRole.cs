namespace EasyStocks.Domain.Entities.Auth;

public class UserRole
{
    public int UserId { get; set; }
    public User User { get; set; } = default!;

    public int RoleId { get; set; }
    public Role Role { get; set; } = default!;
}
