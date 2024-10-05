using Microsoft.Extensions.Options;

namespace eBid.BiddingProcessor.Services;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IEventBus _eventBus;
    private readonly NpgsqlDataSource _dataSource;
    private readonly BackgroundTaskOptions _options;

    public Worker(
        ILogger<Worker> logger,
        IEventBus eventBus,
        NpgsqlDataSource dataSource,
        IOptions<BackgroundTaskOptions> options)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        _dataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("BiddingProcessor background task is doing background work.");
            }

            await using var conn = _dataSource.CreateConnection();
            await using var command = conn.CreateCommand();
            command.CommandText = "SELECT 1";
            
            try
            {
                await conn.OpenAsync(stoppingToken);
                await command.ExecuteNonQueryAsync(stoppingToken);
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(ex, "An error occurred while accessing the database.");
                // Optionally, add retry logic here
            }
            await Task.Delay(TimeSpan.FromSeconds(_options.CheckUpdateTime), stoppingToken);
        }
    }
}