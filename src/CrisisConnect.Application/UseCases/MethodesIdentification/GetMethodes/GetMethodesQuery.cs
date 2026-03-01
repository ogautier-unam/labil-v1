using CrisisConnect.Application.DTOs;
using MediatR;

namespace CrisisConnect.Application.UseCases.MethodesIdentification.GetMethodes;

public record GetMethodesQuery(Guid PersonneId) : IRequest<IReadOnlyList<MethodeIdentificationDto>>;
