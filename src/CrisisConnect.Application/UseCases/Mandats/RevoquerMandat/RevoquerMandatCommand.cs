using MediatR;

namespace CrisisConnect.Application.UseCases.Mandats.RevoquerMandat;

public record RevoquerMandatCommand(Guid MandatId) : IRequest;
