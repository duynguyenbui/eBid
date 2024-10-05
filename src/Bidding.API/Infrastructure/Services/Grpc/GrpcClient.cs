namespace eBid.Bidding.API.Infrastructure.Services.Grpc;

public class GrpcClient
{
    private readonly IConfiguration _configuration;
    private readonly GrpcAuction.GrpcAuctionClient _client;
    private readonly ILogger<GrpcClient> _logger;

    public GrpcClient(IConfiguration configuration, ILogger<GrpcClient> logger)
    {
        _configuration = configuration;
        _logger = logger;
        var channel = GrpcChannel.ForAddress(_configuration["GrpcServer"]!);
        _client = new GrpcAuction.GrpcAuctionClient(channel);
    }

    public AuctionItem? GetGrpcAuction(int id)
    {
        var request = new GetAuctionRequest { Id = id };

        try
        {
            var response = _client.GetAuction(request);

            if (response?.Auction is null)
            {
                return null;
            }

            _logger.LogInformation("Successfully fetched auction with ID: {AuctionId}", response.Auction.Id);

            return new AuctionItem
            {
                Id = response.Auction.Id,
                AuctionEnd = DateTime.Parse(response.Auction.AuctionEnd).ToUniversalTime(),
                Seller = response.Auction.Seller,
                StartingPrice = decimal.Parse(response.Auction.StartingPrice)
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
}