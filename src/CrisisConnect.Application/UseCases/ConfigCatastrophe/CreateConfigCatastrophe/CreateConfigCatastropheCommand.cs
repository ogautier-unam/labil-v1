using CrisisConnect.Application.DTOs;
using Mediator;

namespace CrisisConnect.Application.UseCases.ConfigCatastrophe.CreateConfigCatastrophe;

public record CreateConfigCatastropheCommand(
    string Nom,
    string Description,
    string ZoneGeographique,
    string EtatReferent,
    int DelaiArchivageJours = 30,
    int DelaiRappelAvantArchivage = 7) : IRequest<ConfigCatastropheDto>;
