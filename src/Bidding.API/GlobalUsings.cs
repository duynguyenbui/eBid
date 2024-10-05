// Global using directives

global using eBid.Auction.API.GRPC;
global using eBid.Bidding.API.Apis;
global using eBid.Bidding.API.Application.Behaviors;
global using eBid.Bidding.API.Application.Command;
global using eBid.Bidding.API.Application.IntegrationEvents;
global using eBid.Bidding.API.Application.Queries;
global using eBid.Bidding.API.Application.Validations;
global using eBid.Bidding.API.Infrastructure;
global using eBid.Bidding.API.Infrastructure.Services;
global using eBid.Bidding.API.Infrastructure.Services.Grpc;
global using eBid.Bidding.Domain.AggregatesModel.BiddingAggregate;
global using eBid.Bidding.Domain.Exceptions;
global using eBid.Bidding.Domain.SeedWork;
global using eBid.Bidding.Infrastructure;
global using eBid.Bidding.Infrastructure.Idempotency;
global using eBid.Bidding.Infrastructure.Repositories;
global using eBid.EventBus.Abstractions;
global using eBid.EventBus.Events;
global using eBid.EventBus.Extensions;
global using eBid.IntegrationEventLogEF.Services;
global using eBid.ServiceDefaults;

global using FluentValidation;

global using Grpc.Net.Client;

global using MediatR;

global using Microsoft.AspNetCore.Http.HttpResults;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.EntityFrameworkCore;