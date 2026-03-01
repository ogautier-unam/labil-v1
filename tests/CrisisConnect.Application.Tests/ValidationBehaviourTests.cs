using CrisisConnect.Application.Common.Behaviours;
using FluentValidation;
using MediatR;

namespace CrisisConnect.Application.Tests;

// Types de test pour le ValidationBehaviour
public record ValidationTestCommand(string Nom) : IRequest<string>;

public class ValidationTestCommandValidatorOK : AbstractValidator<ValidationTestCommand>
{
    public ValidationTestCommandValidatorOK()
    {
        RuleFor(x => x.Nom).NotEmpty();
    }
}

public class ValidationTestCommandValidatorKO : AbstractValidator<ValidationTestCommand>
{
    public ValidationTestCommandValidatorKO()
    {
        RuleFor(x => x.Nom).Must(_ => false).WithMessage("Toujours invalide");
    }
}

public class ValidationBehaviourTests
{
    [Fact]
    public async Task Handle_AucunValidateur_AppelleNextEtRetourneRésultat()
    {
        // Arrange
        var behaviour = new ValidationBehaviour<ValidationTestCommand, string>(
            Array.Empty<IValidator<ValidationTestCommand>>());
        RequestHandlerDelegate<string> next = _ => Task.FromResult("ok");

        // Act
        var result = await behaviour.Handle(new ValidationTestCommand("Alice"), next, CancellationToken.None);

        // Assert
        Assert.Equal("ok", result);
    }

    [Fact]
    public async Task Handle_ValidateurPassant_AppelleNextEtRetourneRésultat()
    {
        // Arrange
        var behaviour = new ValidationBehaviour<ValidationTestCommand, string>(
            [new ValidationTestCommandValidatorOK()]);
        RequestHandlerDelegate<string> next = _ => Task.FromResult("ok");

        // Act
        var result = await behaviour.Handle(new ValidationTestCommand("Alice"), next, CancellationToken.None);

        // Assert
        Assert.Equal("ok", result);
    }

    [Fact]
    public async Task Handle_ValidateurÉchouant_LèveValidationException()
    {
        // Arrange
        var behaviour = new ValidationBehaviour<ValidationTestCommand, string>(
            [new ValidationTestCommandValidatorKO()]);
        var nextCalled = false;
        RequestHandlerDelegate<string> next = _ =>
        {
            nextCalled = true;
            return Task.FromResult("ne doit pas être appelé");
        };

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() =>
            behaviour.Handle(new ValidationTestCommand("Alice"), next, CancellationToken.None));
        Assert.False(nextCalled);
    }

    [Fact]
    public async Task Handle_MultipleValidateursKO_RegroupeToutesLesErreurs()
    {
        // Arrange — deux validateurs qui échouent chacun sur une propriété
        var behaviour = new ValidationBehaviour<ValidationTestCommand, string>(
            [new ValidationTestCommandValidatorKO(), new ValidationTestCommandValidatorKO()]);
        RequestHandlerDelegate<string> next = _ => Task.FromResult("ok");

        // Act
        var exception = await Assert.ThrowsAsync<ValidationException>(() =>
            behaviour.Handle(new ValidationTestCommand("Alice"), next, CancellationToken.None));

        // Assert — au moins une erreur par validateur (le context partagé peut en accumuler plus)
        Assert.True(exception.Errors.Count() >= 2);
    }

    [Fact]
    public async Task Handle_CommandeInvalide_MessageErreurCohérent()
    {
        // Arrange — commande avec Nom vide → validateur OK doit échouer
        var behaviour = new ValidationBehaviour<ValidationTestCommand, string>(
            [new ValidationTestCommandValidatorOK()]);
        RequestHandlerDelegate<string> next = _ => Task.FromResult("ok");

        // Act
        var exception = await Assert.ThrowsAsync<ValidationException>(() =>
            behaviour.Handle(new ValidationTestCommand(string.Empty), next, CancellationToken.None));

        // Assert
        Assert.Contains(exception.Errors, e => e.PropertyName == "Nom");
    }
}
