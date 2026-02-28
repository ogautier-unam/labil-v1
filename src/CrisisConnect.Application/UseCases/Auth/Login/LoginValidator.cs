using FluentValidation;

namespace CrisisConnect.Application.UseCases.Auth.Login;

public class LoginValidator : AbstractValidator<LoginCommand>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.MotDePasse).NotEmpty();
    }
}
