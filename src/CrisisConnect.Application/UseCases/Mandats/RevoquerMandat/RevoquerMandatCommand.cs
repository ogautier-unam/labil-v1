using Mediator;

namespace CrisisConnect.Application.UseCases.Mandats.RevoquerMandat;

public record RevoquerMandatCommand(Guid MandatId) : ICommand;
