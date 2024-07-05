﻿namespace EasyStocks.DTO.Requests;

public class UserRequest
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string OtherNames { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string MobileNumber { get; set; } = string.Empty;
    public Gender Gender { get; set; }
    public string PositionInOrg { get; set; } = string.Empty;
    public DateOnly DateOfEmployment { get; set; }
}
