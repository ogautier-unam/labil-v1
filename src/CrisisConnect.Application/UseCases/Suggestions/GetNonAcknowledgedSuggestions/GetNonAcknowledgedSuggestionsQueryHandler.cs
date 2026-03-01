using AutoMapper;
using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Interfaces.Repositories;
using MediatR;

namespace CrisisConnect.Application.UseCases.Suggestions.GetNonAcknowledgedSuggestions;

public class GetNonAcknowledgedSuggestionsQueryHandler
    : IRequestHandler<GetNonAcknowledgedSuggestionsQuery, IReadOnlyList<SuggestionAppariementDto>>
{
    private readonly ISuggestionAppariementRepository _suggestionRepository;
    private readonly IMapper _mapper;

    public GetNonAcknowledgedSuggestionsQueryHandler(
        ISuggestionAppariementRepository suggestionRepository,
        IMapper mapper)
    {
        _suggestionRepository = suggestionRepository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<SuggestionAppariementDto>> Handle(
        GetNonAcknowledgedSuggestionsQuery request,
        CancellationToken cancellationToken)
    {
        var suggestions = await _suggestionRepository.GetNonAcknowledgedAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<SuggestionAppariementDto>>(suggestions);
    }
}
