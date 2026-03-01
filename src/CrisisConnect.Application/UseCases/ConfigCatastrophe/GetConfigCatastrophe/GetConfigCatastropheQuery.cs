using CrisisConnect.Application.DTOs;
using Mediator;

namespace CrisisConnect.Application.UseCases.ConfigCatastrophe.GetConfigCatastrophe;

public record GetConfigCatastropheQuery() : IRequest<ConfigCatastropheDto?>;
