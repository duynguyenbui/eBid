namespace eBid.Bidding.API.Application.Validations;

public class IdentifiedCommandValidator : AbstractValidator<IdentifiedCommand<CreateBiddingCommand, bool>>
{
    public IdentifiedCommandValidator(ILogger<IdentifiedCommandValidator> logger)
    {
        RuleFor(command => command.Id).NotEmpty();

        if (logger.IsEnabled(LogLevel.Trace))
        {
            logger.LogTrace("INSTANCE CREATED - {ClassName}", GetType().Name);
        }
    }
}