using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Enums;
using MediatR;

namespace CrisisConnect.Application.UseCases.Roles.AttribuerRole;

public record AttribuerRoleCommand(
    Guid ActeurId,
    TypeRole TypeRole,
    DateTime DateDebut,
    DateTime? DateFin = null,
    bool Reconductible = false,
    Guid? AccrediteeParId = null) : IRequest<AttributionRoleDto>;
