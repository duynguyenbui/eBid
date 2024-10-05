namespace eBid.Bidding.API.Application.Validations;

public class CreateBiddingCommandValidator : AbstractValidator<CreateBiddingCommand>
{
    public CreateBiddingCommandValidator(ILogger<CreateBiddingCommand> logger)
    {
        RuleFor(command => command.auctionId).NotEmpty();
        RuleFor(command => command.amount).NotEmpty();

        if (logger.IsEnabled(LogLevel.Trace))
        {
            logger.LogTrace("INSTANCE CREATED - {ClassName}", GetType().Name);
        }
    }
}