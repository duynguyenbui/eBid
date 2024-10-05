var builder = DistributedApplication.CreateBuilder(args);

// Infrastructure
var redis = builder.AddRedis("redis");

var postgres = builder.AddPostgres("postgres")
    .WithImage("pgvector/pgvector").WithImageTag("pg16")
    .WithBindMount("../../postgres_init.sql", "/docker-entrypoint-initdb.d/postgres_init.sql");

var rabbitMq = builder.AddRabbitMQ("eventbus");

var elasticsearch = builder.AddElasticsearch("elasticsearch");

// Prometheus and Grafana (uncomment for monitoring setup)
// if (isDevelopment)
// {
//     var grafana = builder.AddContainer("grafana", "grafana/grafana")
//         .WithBindMount("../../grafana/config", "/etc/grafana", isReadOnly: true)
//         .WithBindMount("../../grafana/dashboards", "/var/lib/grafana/dashboards", isReadOnly: true)
//         .WithHttpEndpoint(targetPort: 3000, name: "http");
//     builder.AddContainer("prometheus", "prom/prometheus")
//         .WithBindMount("../../prometheus", "/etc/prometheus", isReadOnly: true)
//         .WithHttpEndpoint(port: 9090, targetPort: 9090);
// }

// Databases
var identityDb = postgres.AddDatabase("identitydb");
var auctionDb = postgres.AddDatabase("auctiondb");
var biddingDb = postgres.AddDatabase("biddingdb");


// Configure main components of the application
var identityApi = builder.AddProject<Projects.Identity_API>("identity-api")
    .WithReference(identityDb);

var identityEndpoint = identityApi.GetEndpoint("http");

var auctionApi = builder.AddProject<Projects.Auction_API>("auction-api")
    .WithReference(rabbitMq)
    .WithReference(auctionDb)
    .WithEnvironment("Identity__Url",
        // "http://identity-api:5000"
        identityEndpoint
    );

var searchApi = builder.AddProject<Projects.Search_API>("search-api")
    .WithReference(redis)
    .WithReference(elasticsearch)
    .WithReference(rabbitMq);

// Other projects (uncomment and configure as needed)
builder.AddProject<Projects.BiddingProcessor>("bidding-processor")
    .WithReference(biddingDb)
    .WithReference(rabbitMq);

var biddingApi = builder.AddProject<Projects.Bidding_API>("bidding-api")
    .WithReference(biddingDb)
    .WithReference(rabbitMq)
    .WithEnvironment("Identity__Url",
        // "http://identity-api:5000"
        identityEndpoint
    );

// var paymentProcessor = builder.AddProject<Projects.PaymentProcessor>("payment-processor");
// var bff = builder.AddProject<Projects.BFF>("bff");

// Configuration for OpenAI
const bool useOpenAI = true;
if (useOpenAI)
{
    var openAI = builder.Configuration.GetSection("OpenAIOptions");

    if (openAI.Exists())
    {
        string apiKey = builder.Configuration["OpenAIOptions:ApiKey"]!;
        string model = builder.Configuration["OpenAIOptions:Model"]!;
        string endpoint = builder.Configuration["OpenAIOptions:Endpoint"]!;

        // Assign variables to the specific project 
        auctionApi.WithEnvironment("OpenAIOptions__ApiKey", apiKey)
            .WithEnvironment("OpenAIOptions__Model", model)
            .WithEnvironment("OpenAIOptions__Endpoint", endpoint);
    }
}

// Cloudinary configuration
auctionApi.WithEnvironment("CloudinaryOptions__CloudName", builder.Configuration["CloudinaryOptions:CloudName"])
    .WithEnvironment("CloudinaryOptions__ApiKey", builder.Configuration["CloudinaryOptions:ApiKey"])
    .WithEnvironment("CloudinaryOptions__ApiSecret", builder.Configuration["CloudinaryOptions:ApiSecret"]);

biddingApi.WithEnvironment("GrpcServer", auctionApi.GetEndpoint("http2"));

// Setup identity API to communicate with auction API
identityApi.WithEnvironment("AuctionApiClient",
        auctionApi.GetEndpoint("http")
        // "http://auction-api:5001"
    )
    .WithEnvironment("BiddingApiClient",
        biddingApi.GetEndpoint("http")
        // "http://bidding-api:5002"
    )
    .WithEnvironment("WebAppClient", "http://localhost:3000");


// Just implement in development mode for now 
// builder.AddNpmApp("webapp", "../webapp")
//     .WithEnvironment("NEXT_PUBLIC_SEARCH_URL", searchApi.GetEndpoint("http"))
//     .WithHttpEndpoint(env: "PORT")
//     .WithExternalHttpEndpoints()
//     .PublishAsDockerFile();

builder.Build().Run();