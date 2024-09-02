using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EasyStocks.DTO.Requests;

public class RegisterAdminRequest
{
    [Required]
    public string FirstName { get; set; } = string.Empty;
    [Required]
    public string LastName { get; set; } = string.Empty;
    [Required]
    public string OtherNames { get; set; } = string.Empty;
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string PhoneNumber { get; set; } = string.Empty;
    [Required]
    public Gender Gender { get; set; }
    [Required]
    [PasswordPropertyText]
    public string Password { get; set; } = string.Empty;
    [Required]
    public string ConfirmPassword { get; set; } = string.Empty;
}