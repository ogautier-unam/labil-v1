using CrisisConnect.Application.Mappings;
using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Interfaces.Repositories;
using CrisisConnect.Domain.Interfaces.Services;
using Mediator;

namespace CrisisConnect.Application.UseCases.Entites.CreateEntite;

public class CreateEntiteCommandHandler : IRequestHandler<CreateEntiteCommand, EntiteDto>
{
    private readonly IEntiteRepository _repository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly AppMapper _mapper;

    public CreateEntiteCommandHandler(IEntiteRepository repository, IPasswordHasher passwordHasher, AppMapper mapper)
    {
        _repository = repository;
        _passwordHasher = passwordHasher;
        _mapper = mapper;
    }

    public async ValueTask<EntiteDto> Handle(CreateEntiteCommand request, CancellationToken cancellationToken)
    {
        var hash = _passwordHasher.Hacher(request.MotDePasse);
        var entite = new Entite(request.Email, hash, request.Nom, request.Description, request.MoyensContact, request.ResponsableId);

        await _repository.AddAsync(entite, cancellationToken);
        return _mapper.ToDto(entite);
    }
}
