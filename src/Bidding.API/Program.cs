var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddApplicationServices();
var withApiVersioning = builder.Services.AddApiVersioning();
builder.AddDefaultOpenApi(withApiVersioning);

var app = builder.Build();

app.MapDefaultEndpoints();

var bidding = app.NewVersionedApi("Bidding");
bidding.MapBiddingApiV1();

app.UseDefaultOpenApi();

app.Run();