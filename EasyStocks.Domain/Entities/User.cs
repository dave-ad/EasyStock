﻿namespace EasyStocks.Domain.Entities;

public class User : Entity
{
    public int UserId { get; set; }
    public FullName Name { get; private set; } = FullName.Default();
    public Email Email { get; private set; } = Email.Default();
    public MobileNo MobileNumber { get; private set; } = MobileNo.Default();
    public Gender Gender { get; private set; }
    public string? PositionInOrg { get; private set; } = default!;
    public DateOnly? DateOfEmployment { get; private set; }

    // Navigation property to Broker
    public int BrokerId { get; set; } // Foreign key
    public Broker Broker { get; set; } // Navigation property

    private User() { }
    private User(FullName name, Email email, MobileNo mobileNumber, Gender gender, string? positionInOrg, DateOnly? dateOfEmployment)
    {
        Name = name;
        Email = email;
        MobileNumber = mobileNumber;
        Gender = gender;
        PositionInOrg = positionInOrg;
        DateOfEmployment = dateOfEmployment;
    }

    public static User Create(FullName name, Email email, MobileNo mobileNumber, Gender gender, string? positionInOrg, DateOnly? dateOfEmployment)
    {
        return new User(name, email, mobileNumber, gender, positionInOrg, dateOfEmployment);
    }
}