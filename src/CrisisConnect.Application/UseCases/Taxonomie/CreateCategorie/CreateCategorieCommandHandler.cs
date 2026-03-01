using AutoMapper;
using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Interfaces.Repositories;
using MediatR;

namespace CrisisConnect.Application.UseCases.Taxonomie.CreateCategorie;

public class CreateCategorieCommandHandler : IRequestHandler<CreateCategorieCommand, CategorieTaxonomieDto>
{
    private readonly ICategorieTaxonomieRepository _repository;
    private readonly IMapper _mapper;

    public CreateCategorieCommandHandler(ICategorieTaxonomieRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CategorieTaxonomieDto> Handle(CreateCategorieCommand request, CancellationToken cancellationToken)
    {
        var categorie = new CategorieTaxonomie(
            request.Code,
            request.NomJson,
            request.ConfigId,
            request.ParentId,
            request.DescriptionJson);

        await _repository.AddAsync(categorie, cancellationToken);
        return _mapper.Map<CategorieTaxonomieDto>(categorie);
    }
}
