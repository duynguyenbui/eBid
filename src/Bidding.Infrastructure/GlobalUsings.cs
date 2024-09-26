// Global using directives

global using System.ComponentModel.DataAnnotations;
global using System.Data;

global using eBid.Bidding.Domain.AggregatesModel.BuyerAggregate;
global using eBid.Bidding.Domain.Exceptions;
global using eBid.Bidding.Domain.SeedWork;
global using eBid.Bidding.Infrastructure.EntityConfigurations;
global using eBid.Bidding.Infrastructure.Idempotency;
global using eBid.IntegrationEventLogEF;

global using MediatR;

global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using Microsoft.EntityFrameworkCore.Storage;