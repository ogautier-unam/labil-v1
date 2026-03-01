using AutoMapper;
using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Interfaces.Repositories;
using MediatR;

namespace CrisisConnect.Application.UseCases.Taxonomie.GetCategories;

public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, IReadOnlyList<CategorieTaxonomieDto>>
{
    private readonly ICategorieTaxonomieRepository _repository;
    private readonly IMapper _mapper;

    public GetCategoriesQueryHandler(ICategorieTaxonomieRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<CategorieTaxonomieDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _repository.GetRacinesAsync(request.ConfigId, cancellationToken);
        return _mapper.Map<IReadOnlyList<CategorieTaxonomieDto>>(categories);
    }
}
