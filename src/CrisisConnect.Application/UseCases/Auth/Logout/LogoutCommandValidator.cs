using FluentValidation;

namespace CrisisConnect.Application.UseCases.Auth.Logout;

public class LogoutCommandValidator : AbstractValidator<LogoutCommand>
{
    public LogoutCommandValidator()
    {
        RuleFor(x => x.PersonneId).NotEmpty();
    }
}
