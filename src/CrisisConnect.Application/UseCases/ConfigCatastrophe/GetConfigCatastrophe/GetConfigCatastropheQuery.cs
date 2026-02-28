using CrisisConnect.Application.DTOs;
using MediatR;

namespace CrisisConnect.Application.UseCases.ConfigCatastrophe.GetConfigCatastrophe;

public record GetConfigCatastropheQuery() : IRequest<ConfigCatastropheDto?>;
