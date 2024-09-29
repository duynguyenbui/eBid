var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddApplicationServices();
builder.Services.AddProblemDetails();

var withApiVersioning =
    builder.Services.AddApiVersioning(options => options.AssumeDefaultVersionWhenUnspecified = true);

builder.AddDefaultOpenApi(withApiVersioning);

var app = builder.Build();

app.MapDefaultEndpoints();
app.NewVersionedApi("Auction")
    .MapAuctionApiV1();

app.UseDefaultOpenApi();
app.Run();