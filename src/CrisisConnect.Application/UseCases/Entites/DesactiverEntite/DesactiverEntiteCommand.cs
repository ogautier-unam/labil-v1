using Mediator;

namespace CrisisConnect.Application.UseCases.Entites.DesactiverEntite;

public record DesactiverEntiteCommand(Guid EntiteId) : ICommand;
