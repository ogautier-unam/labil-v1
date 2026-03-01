using CrisisConnect.Application.Mappings;
using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Interfaces.Repositories;
using Mediator;

namespace CrisisConnect.Application.UseCases.MethodesIdentification.GetMethodes;

public class GetMethodesQueryHandler : IRequestHandler<GetMethodesQuery, IReadOnlyList<MethodeIdentificationDto>>
{
    private readonly IMethodeIdentificationRepository _repository;
    private readonly AppMapper _mapper;

    public GetMethodesQueryHandler(IMethodeIdentificationRepository repository, AppMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async ValueTask<IReadOnlyList<MethodeIdentificationDto>> Handle(GetMethodesQuery request, CancellationToken cancellationToken)
    {
        var methodes = await _repository.GetByPersonneAsync(request.PersonneId, cancellationToken);
        return _mapper.ToDto(methodes);
    }
}
