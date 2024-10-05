global using System.Text.Json;
global using System.Text.Json.Serialization;

global using eBid.EventBus.Abstractions;
global using eBid.EventBus.Events;
global using eBid.Search.API;
global using eBid.Search.API.Constants;
global using eBid.Search.API.Extensions;
global using eBid.Search.API.IntegrationEvents.EventHandling;
global using eBid.Search.API.IntegrationEvents.Events;
global using eBid.Search.API.Model;
global using eBid.Search.API.Repositories;
global using eBid.Search.API.Repositories.Caching;
global using eBid.Search.API.Services;
global using eBid.ServiceDefaults;

global using Elastic.Clients.Elasticsearch;

global using Microsoft.AspNetCore.Http.HttpResults;
global using Microsoft.AspNetCore.Mvc;

global using StackExchange.Redis;