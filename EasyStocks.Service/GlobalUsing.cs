﻿global using EasyStocks.Domain.Entities;
global using EasyStocks.Domain.ValueObjects;
global using EasyStocks.DTO.Common;
global using EasyStocks.DTO.Requests;
global using EasyStocks.DTO.Responses;
global using EasyStocks.Infrastructure;
global using EasyStocks.Infrastructure.Validators;
global using EasyStocks.Utils.Enums;
global using EasyStocks.Service.AdminAuthServices;
global using EasyStocks.Service.AuthServices;
global using EasyStocks.Service.BrokerServices;
global using EasyStocks.Service.StocksServices;
global using EasyStocks.Service.TokenServices;
global using EasyStocks.Service.UserAuthServices;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;
global using System.Transactions;
