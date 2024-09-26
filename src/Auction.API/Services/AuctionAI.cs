namespace eBid.Auction.API.Services;

public sealed class AuctionAI : IAuctionAI
{
    private const int EmbeddingDimensions = 768;

    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<AuctionAI> _logger;
    private readonly OpenAIClient _openAIClient;
    private readonly OpenAIOptions _options;

    public AuctionAI(IWebHostEnvironment environment, ILogger<AuctionAI> logger,
        IOptions<OpenAIOptions> options, OpenAIClient openAIClient = null)
    {
        _environment = environment;
        _logger = logger;
        _openAIClient = openAIClient;
        _options = options.Value;
    }

    public bool IsEnabled => _openAIClient is not null;

    public async ValueTask<Vector?> GetEmbeddingAsync(string text)
    {
        if (!IsEnabled)
        {
            _logger.LogWarning("Embedding is not enabled. Returning default vector.");
            return null;
        }

        long timestamp = Stopwatch.GetTimestamp();

        try
        {
            var embeddingResponse = await _openAIClient.EmbeddingsEndpoint.CreateEmbeddingAsync(
                text, _options.Model, dimensions: EmbeddingDimensions);

            var embedding = embeddingResponse.Data?
                .FirstOrDefault()?.Embedding.Select(e => (float)e)
                .ToArray() ?? new float[EmbeddingDimensions];

            _logger.LogTrace("Generated embedding in {ElapsedMilliseconds}ms: '{Text}'",
                Stopwatch.GetElapsedTime(timestamp).TotalMilliseconds, text);

            return new Vector(new ReadOnlyMemory<float>(embedding));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to generate embedding. Returning default vector.");
            return null;
        }
    }

    public async ValueTask<Vector?> GetEmbeddingAsync(AuctionItem item)
        => IsEnabled ? await GetEmbeddingAsync(AuctionItemToString(item)) : null;

    public async ValueTask<IReadOnlyList<Vector>> GetEmbeddingsAsync(IEnumerable<AuctionItem> items)
    {
        if (IsEnabled)
        {
            long timestamp = Stopwatch.GetTimestamp();

            var embeddings = new List<Vector>();

            foreach (var item in items)
            {
                var embedding = await GetEmbeddingAsync(item);
                if (embedding is not null)
                {
                    embeddings.Add(embedding);
                }
            }

            _logger.LogTrace("Generated {EmbeddingsCount} embeddings in {ElapsedMilliseconds}ms", embeddings.Count,
                Stopwatch.GetElapsedTime(timestamp).TotalMilliseconds);

            return embeddings;
        }

        _logger.LogWarning("Embedding is not enabled. Returning empty list of vectors.");
        return new List<Vector>(); // Return an empty list instead of null
    }

    private static string AuctionItemToString(AuctionItem item)
        => $"{item.Name} {item.Description}";
}