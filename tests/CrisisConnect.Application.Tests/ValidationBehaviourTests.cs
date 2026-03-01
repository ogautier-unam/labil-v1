using CrisisConnect.Application.Common.Behaviours;
using FluentValidation;
using Mediator;

namespace CrisisConnect.Application.Tests;

// Types de test pour le ValidationBehaviour
public record ValidationTestCommand(string Nom) : ICommand<string>;

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
        MessageHandlerDelegate<ValidationTestCommand, string> next = (_, _) => ValueTask.FromResult("ok");

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
        MessageHandlerDelegate<ValidationTestCommand, string> next = (_, _) => ValueTask.FromResult("ok");

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
        MessageHandlerDelegate<ValidationTestCommand, string> next = (_, _) =>
        {
            nextCalled = true;
            return ValueTask.FromResult("ne doit pas être appelé");
        };

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() =>
            behaviour.Handle(new ValidationTestCommand("Alice"), next, CancellationToken.None).AsTask());
        Assert.False(nextCalled);
    }

    [Fact]
    public async Task Handle_MultipleValidateursKO_RegroupeToutesLesErreurs()
    {
        // Arrange — deux validateurs qui échouent chacun sur une propriété
        var behaviour = new ValidationBehaviour<ValidationTestCommand, string>(
            [new ValidationTestCommandValidatorKO(), new ValidationTestCommandValidatorKO()]);
        MessageHandlerDelegate<ValidationTestCommand, string> next = (_, _) => ValueTask.FromResult("ok");

        // Act
        var exception = await Assert.ThrowsAsync<ValidationException>(() =>
            behaviour.Handle(new ValidationTestCommand("Alice"), next, CancellationToken.None).AsTask());

        // Assert — au moins une erreur par validateur (le context partagé peut en accumuler plus)
        Assert.True(exception.Errors.Count() >= 2);
    }

    [Fact]
    public async Task Handle_CommandeInvalide_MessageErreurCohérent()
    {
        // Arrange — commande avec Nom vide → validateur OK doit échouer
        var behaviour = new ValidationBehaviour<ValidationTestCommand, string>(
            [new ValidationTestCommandValidatorOK()]);
        MessageHandlerDelegate<ValidationTestCommand, string> next = (_, _) => ValueTask.FromResult("ok");

        // Act
        var exception = await Assert.ThrowsAsync<ValidationException>(() =>
            behaviour.Handle(new ValidationTestCommand(string.Empty), next, CancellationToken.None).AsTask());

        // Assert
        Assert.Contains(exception.Errors, e => e.PropertyName == "Nom");
    }
}
