namespace eBid.Bidding.Infrastructure;

/// <remarks>
/// Add migrations using the following command inside the 'Bidding.Infrastructure' project directory:
///
/// dotnet ef migrations add --startup-project ../Bidding.API/ --context BiddingContext [migration-name]
/// </remarks>
public class BiddingContext : DbContext, IUnitOfWork
{
    public DbSet<PaymentMethod> Payments { get; set; }
    public DbSet<Buyer> Buyers { get; set; }
    public DbSet<CardType> CardTypes { get; set; }

    public DbSet<Bid> Bids { get; set; }
    
    public DbSet<AuctionItem> AuctionItems { get; set; }

    private readonly IMediator _mediator;
    private IDbContextTransaction _currentTransaction;

    public BiddingContext(DbContextOptions<BiddingContext> options) : base(options) { }

    public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;

    public bool HasActiveTransaction => _currentTransaction != null;

    public BiddingContext(DbContextOptions<BiddingContext> options, IMediator mediator) : base(options)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        System.Diagnostics.Debug.WriteLine("BiddingContext::ctor ->" + this.GetHashCode());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ClientRequestEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new PaymentMethodEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CardTypeEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new BuyerEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new BidEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new AuctionItemEntityTypeConfiguration());
        
        modelBuilder.UseIntegrationEventLogs();
    }

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        // Dispatch Domain Events collection. 
        // Choices:
        // A) Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including  
        // side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
        // B) Right AFTER committing data (EF SaveChanges) into the DB will make multiple transactions. 
        // You will need to handle eventual consistency and compensatory actions in case of failures in any of the Handlers. 
        await _mediator.DispatchDomainEventsAsync(this);

        _ = await base.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        if (_currentTransaction != null) return null;

        _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

        return _currentTransaction;
    }

    public async Task CommitTransactionAsync(IDbContextTransaction transaction)
    {
        if (transaction == null) throw new ArgumentNullException(nameof(transaction));
        if (transaction != _currentTransaction)
            throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

        try
        {
            await SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            RollbackTransaction();
            throw;
        }
        finally
        {
            if (HasActiveTransaction)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public void RollbackTransaction()
    {
        try
        {
            _currentTransaction?.Rollback();
        }
        finally
        {
            if (HasActiveTransaction)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }
}

#nullable enable