using Mediator;

namespace CrisisConnect.Application.UseCases.MethodesIdentification.VerifierMethode;

public record VerifierMethodeCommand(Guid MethodeId) : ICommand;
