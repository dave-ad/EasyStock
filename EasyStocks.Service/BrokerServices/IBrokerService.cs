﻿namespace EasyStocks.Service.BrokerServices;

public interface IBrokerService
{
    Task<ServiceResponse<BrokerIdResponse>> CreateCorporateBroker(CreateCorporateBrokerRequest request);
    Task<ServiceResponse<BrokerIdResponse>> CreateIndividualBroker(CreateIndividualBrokerRequest request);
    Task<ServiceResponse<BrokerIdResponse>> CreateFreelanceBroker(CreateFreelanceBrokerRequest request);

    Task<ServiceResponse<BrokerListResponse>> GetAllBrokers();
    Task<ServiceResponse<BrokerResponse>> GetBrokerById(int id);
}