using CrisisConnect.Application.DTOs;
using Mediator;

namespace CrisisConnect.Application.UseCases.Acteurs.GetActeur;

public record GetActeurQuery(Guid Id) : IQuery<PersonneDto>;
