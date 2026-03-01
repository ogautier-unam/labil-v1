using CrisisConnect.Application.DTOs;
using Mediator;

namespace CrisisConnect.Application.UseCases.ConfigCatastrophe.UpdateConfigCatastrophe;

public record UpdateConfigCatastropheCommand(
    Guid Id,
    int DelaiArchivageJours,
    int DelaiRappelAvantArchivage,
    bool EstActive) : IRequest<ConfigCatastropheDto>;
