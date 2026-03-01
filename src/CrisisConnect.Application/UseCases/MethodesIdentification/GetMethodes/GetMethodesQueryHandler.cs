using AutoMapper;
using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Interfaces.Repositories;
using MediatR;

namespace CrisisConnect.Application.UseCases.MethodesIdentification.GetMethodes;

public class GetMethodesQueryHandler : IRequestHandler<GetMethodesQuery, IReadOnlyList<MethodeIdentificationDto>>
{
    private readonly IMethodeIdentificationRepository _repository;
    private readonly IMapper _mapper;

    public GetMethodesQueryHandler(IMethodeIdentificationRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<MethodeIdentificationDto>> Handle(GetMethodesQuery request, CancellationToken cancellationToken)
    {
        var methodes = await _repository.GetByPersonneAsync(request.PersonneId, cancellationToken);
        return _mapper.Map<IReadOnlyList<MethodeIdentificationDto>>(methodes);
    }
}
