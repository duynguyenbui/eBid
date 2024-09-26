namespace eBid.BiddingProcessor.Services;

public class Worker(ILogger<Worker> logger, NpgsqlDataSource dataSource) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                try
                {
                    await using var conn = dataSource.CreateConnection();
                    await conn.OpenAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to connect to database: {ex.Message}");
                }
            }

            await Task.Delay(5000, stoppingToken);
        }
    }
}