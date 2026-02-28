using AutoMapper;
using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using MediatR;

namespace CrisisConnect.Application.UseCases.Demandes.GetDemandeById;

public class GetDemandeByIdQueryHandler : IRequestHandler<GetDemandeByIdQuery, DemandeDto>
{
    private readonly IDemandeRepository _repository;
    private readonly IMapper _mapper;

    public GetDemandeByIdQueryHandler(IDemandeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<DemandeDto> Handle(GetDemandeByIdQuery request, CancellationToken cancellationToken)
    {
        var demande = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Demande), request.Id);
        return _mapper.Map<DemandeDto>(demande);
    }
}
