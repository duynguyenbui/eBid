var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddApplicationServices();
builder.AddDefaultAuthentication();
var withApiVersioning = builder.Services.AddApiVersioning();
builder.AddDefaultOpenApi(withApiVersioning);
builder.Services.AddProblemDetails();

var app = builder.Build();

app.MapDefaultEndpoints();

var bidding = app.NewVersionedApi("Bidding");
bidding.MapBiddingApiV1().RequireAuthorization();

app.UseDefaultOpenApi();

app.Run();