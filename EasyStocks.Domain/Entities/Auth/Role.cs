namespace EasyStocks.Domain.Entities.Auth;

public class Role
{
    public int RoleId { get; set; }
    public string RoleName { get; set; } = default!;
    public List<UserRole> UserRoles { get; set; } = new();
}