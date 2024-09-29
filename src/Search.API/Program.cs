var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddApplicationServices();
builder.Services.AddProblemDetails();

var withApiVersioning =
    builder.Services.AddApiVersioning(options => options.AssumeDefaultVersionWhenUnspecified = true);

builder.AddDefaultOpenApi(withApiVersioning);

var app = builder.Build();

app.UseCors("CorsPolicy");

app.MapDefaultEndpoints();

app.NewVersionedApi("Search")
    .MapSearchApiV1();

app.MapGet("/elscheck", async ([FromServices] ElasticsearchClient client) =>
{
    var health = await client.Cluster.HealthAsync();
    return health;
}).WithName("elscheck");

app.UseDefaultOpenApi();
app.Run();