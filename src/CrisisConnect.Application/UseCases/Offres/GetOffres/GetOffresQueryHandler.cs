using AutoMapper;
using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Interfaces.Repositories;
using MediatR;

namespace CrisisConnect.Application.UseCases.Offres.GetOffres;

public class GetOffresQueryHandler : IRequestHandler<GetOffresQuery, IReadOnlyList<OffreDto>>
{
    private readonly IOffreRepository _repository;
    private readonly IMapper _mapper;

    public GetOffresQueryHandler(IOffreRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<OffreDto>> Handle(GetOffresQuery request, CancellationToken cancellationToken)
    {
        var offres = await _repository.GetAllAsync(cancellationToken);

        if (request.Statut.HasValue)
            offres = offres.Where(o => o.Statut == request.Statut.Value).ToList();

        return _mapper.Map<IReadOnlyList<OffreDto>>(offres);
    }
}
