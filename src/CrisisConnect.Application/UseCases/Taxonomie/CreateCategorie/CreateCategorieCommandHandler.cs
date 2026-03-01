using CrisisConnect.Application.Mappings;
using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Interfaces.Repositories;
using Mediator;

namespace CrisisConnect.Application.UseCases.Taxonomie.CreateCategorie;

public class CreateCategorieCommandHandler : IRequestHandler<CreateCategorieCommand, CategorieTaxonomieDto>
{
    private readonly ICategorieTaxonomieRepository _repository;
    private readonly AppMapper _mapper;

    public CreateCategorieCommandHandler(ICategorieTaxonomieRepository repository, AppMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async ValueTask<CategorieTaxonomieDto> Handle(CreateCategorieCommand request, CancellationToken cancellationToken)
    {
        var categorie = new CategorieTaxonomie(
            request.Code,
            request.NomJson,
            request.ConfigId,
            request.ParentId,
            request.DescriptionJson);

        await _repository.AddAsync(categorie, cancellationToken);
        return _mapper.ToDto(categorie);
    }
}
