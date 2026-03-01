using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Enums;
using Mediator;

namespace CrisisConnect.Application.UseCases.Mandats.CreerMandat;

public record CreerMandatCommand(
    Guid MandantId,
    Guid MandataireId,
    PorteeMandat Portee,
    string Description,
    bool EstPublic,
    DateTime DateDebut,
    DateTime? DateFin = null) : IRequest<MandatDto>;
