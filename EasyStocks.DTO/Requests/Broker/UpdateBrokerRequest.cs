﻿namespace EasyStocks.DTO.Requests;

public class UpdateBrokerRequest
{
    public int BrokerId { get; set; }

    public string CompanyName { get; set; } = string.Empty;
    public string CompanyEmail { get; set; } = string.Empty;
    public string CompanyMobileNumber { get; set; } = string.Empty;
    // Address Property
    public string StreetNo { get; set; } = string.Empty;
    public string StreetName { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;

    public string CACRegistrationNumber { get; set; } = string.Empty;
    public string StockBrokerLicense { get; set; } = string.Empty;
    public DateOnly? DateCertified { get; set; }
}