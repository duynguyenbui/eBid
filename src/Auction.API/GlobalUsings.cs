global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Options;
global using Microsoft.AspNetCore.Http.HttpResults;
global using Microsoft.AspNetCore.Mvc;

global using eBid.Auction.API.Model.DataTransferObjects;
global using eBid.Auction.API.IntegrationEvents;
global using eBid.Auction.API.Infrastructure;
global using eBid.Auction.API;
global using eBid.Auction.API.Model;
global using eBid.Auction.API.Services;
global using eBid.EventBus.Abstractions;
global using eBid.EventBus.Events;
global using eBid.ServiceDefaults;
global using eBid.IntegrationEventLogEF.Services;
global using eBid.IntegrationEventLogEF.Utilities;
global using eBid.Auction.API.Extensions;
global using eBid.Auction.API.Infrastructure.EntityConfigurations;
global using eBid.Auction.API.IntegrationEvents.Events;
global using eBid.IntegrationEventLogEF;

global using System.ComponentModel.DataAnnotations;
global using System.Diagnostics.CodeAnalysis;
global using System.Text.Json;
global using System.Text.Json.Serialization;
global using System.Diagnostics;

global using CloudinaryDotNet;
global using CloudinaryDotNet.Actions;

global using eBid.Auction.API.Services.AI;
global using eBid.Auction.API.Services.Identity;
global using eBid.Auction.API.Services.Image;

global using Npgsql;

global using OpenAI;

global using Pgvector;
global using Pgvector.EntityFrameworkCore;