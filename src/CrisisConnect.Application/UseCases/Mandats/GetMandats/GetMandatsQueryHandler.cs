using CrisisConnect.Application.Mappings;
using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Interfaces.Repositories;
using Mediator;

namespace CrisisConnect.Application.UseCases.Mandats.GetMandats;

public class GetMandatsQueryHandler : IRequestHandler<GetMandatsQuery, IReadOnlyList<MandatDto>>
{
    private readonly IMandatRepository _repository;
    private readonly AppMapper _mapper;

    public GetMandatsQueryHandler(IMandatRepository repository, AppMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async ValueTask<IReadOnlyList<MandatDto>> Handle(GetMandatsQuery request, CancellationToken cancellationToken)
    {
        var mandats = await _repository.GetByMandantAsync(request.ActeurId, cancellationToken);
        return _mapper.ToDto(mandats);
    }
}
