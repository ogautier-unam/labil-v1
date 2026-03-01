using AutoMapper;
using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Interfaces.Repositories;
using MediatR;

namespace CrisisConnect.Application.UseCases.Mandats.GetMandats;

public class GetMandatsQueryHandler : IRequestHandler<GetMandatsQuery, IReadOnlyList<MandatDto>>
{
    private readonly IMandatRepository _repository;
    private readonly IMapper _mapper;

    public GetMandatsQueryHandler(IMandatRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<MandatDto>> Handle(GetMandatsQuery request, CancellationToken cancellationToken)
    {
        var mandats = await _repository.GetByMandantAsync(request.ActeurId, cancellationToken);
        return _mapper.Map<IReadOnlyList<MandatDto>>(mandats);
    }
}
