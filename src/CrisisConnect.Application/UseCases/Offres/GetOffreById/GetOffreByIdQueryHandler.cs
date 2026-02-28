using AutoMapper;
using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using MediatR;

namespace CrisisConnect.Application.UseCases.Offres.GetOffreById;

public class GetOffreByIdQueryHandler : IRequestHandler<GetOffreByIdQuery, OffreDto>
{
    private readonly IOffreRepository _repository;
    private readonly IMapper _mapper;

    public GetOffreByIdQueryHandler(IOffreRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<OffreDto> Handle(GetOffreByIdQuery request, CancellationToken cancellationToken)
    {
        var offre = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Offre), request.Id);
        return _mapper.Map<OffreDto>(offre);
    }
}
