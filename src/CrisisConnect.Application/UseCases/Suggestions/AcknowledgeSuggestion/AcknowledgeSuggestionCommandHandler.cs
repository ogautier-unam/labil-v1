using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using MediatR;

namespace CrisisConnect.Application.UseCases.Suggestions.AcknowledgeSuggestion;

public class AcknowledgeSuggestionCommandHandler : IRequestHandler<AcknowledgeSuggestionCommand>
{
    private readonly ISuggestionAppariementRepository _suggestionRepository;

    public AcknowledgeSuggestionCommandHandler(ISuggestionAppariementRepository suggestionRepository)
    {
        _suggestionRepository = suggestionRepository;
    }

    public async Task Handle(AcknowledgeSuggestionCommand request, CancellationToken cancellationToken)
    {
        var suggestion = await _suggestionRepository.GetByIdAsync(request.SuggestionId, cancellationToken)
            ?? throw new NotFoundException(nameof(SuggestionAppariement), request.SuggestionId);

        suggestion.Acknowledger();
        await _suggestionRepository.UpdateAsync(suggestion, cancellationToken);
    }
}
