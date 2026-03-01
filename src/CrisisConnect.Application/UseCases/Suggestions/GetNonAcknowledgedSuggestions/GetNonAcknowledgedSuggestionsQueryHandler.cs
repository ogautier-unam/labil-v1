using CrisisConnect.Application.Mappings;
using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Interfaces.Repositories;
using Mediator;

namespace CrisisConnect.Application.UseCases.Suggestions.GetNonAcknowledgedSuggestions;

public class GetNonAcknowledgedSuggestionsQueryHandler
    : IRequestHandler<GetNonAcknowledgedSuggestionsQuery, IReadOnlyList<SuggestionAppariementDto>>
{
    private readonly ISuggestionAppariementRepository _suggestionRepository;
    private readonly AppMapper _mapper;

    public GetNonAcknowledgedSuggestionsQueryHandler(
        ISuggestionAppariementRepository suggestionRepository,
        AppMapper mapper)
    {
        _suggestionRepository = suggestionRepository;
        _mapper = mapper;
    }

    public async ValueTask<IReadOnlyList<SuggestionAppariementDto>> Handle(
        GetNonAcknowledgedSuggestionsQuery request,
        CancellationToken cancellationToken)
    {
        var suggestions = await _suggestionRepository.GetNonAcknowledgedAsync(cancellationToken);
        return _mapper.ToDto(suggestions);
    }
}
