using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using Mediator;

namespace CrisisConnect.Application.UseCases.Suggestions.AcknowledgeSuggestion;

public class AcknowledgeSuggestionCommandHandler : ICommandHandler<AcknowledgeSuggestionCommand>
{
    private readonly ISuggestionAppariementRepository _suggestionRepository;

    public AcknowledgeSuggestionCommandHandler(ISuggestionAppariementRepository suggestionRepository)
    {
        _suggestionRepository = suggestionRepository;
    }

    public async ValueTask<Unit> Handle(AcknowledgeSuggestionCommand request, CancellationToken cancellationToken)
    {
        var suggestion = await _suggestionRepository.GetByIdAsync(request.SuggestionId, cancellationToken)
            ?? throw new NotFoundException(nameof(SuggestionAppariement), request.SuggestionId);

        suggestion.Acknowledger();
        await _suggestionRepository.UpdateAsync(suggestion, cancellationToken);
        return Unit.Value;
    }
}
