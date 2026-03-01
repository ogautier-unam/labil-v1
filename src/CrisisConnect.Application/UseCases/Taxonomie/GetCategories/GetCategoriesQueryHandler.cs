using CrisisConnect.Application.Mappings;
using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Interfaces.Repositories;
using Mediator;

namespace CrisisConnect.Application.UseCases.Taxonomie.GetCategories;

public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, IReadOnlyList<CategorieTaxonomieDto>>
{
    private readonly ICategorieTaxonomieRepository _repository;
    private readonly AppMapper _mapper;

    public GetCategoriesQueryHandler(ICategorieTaxonomieRepository repository, AppMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async ValueTask<IReadOnlyList<CategorieTaxonomieDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _repository.GetRacinesAsync(request.ConfigId, cancellationToken);
        return _mapper.ToDto(categories);
    }
}
