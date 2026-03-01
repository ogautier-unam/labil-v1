using CrisisConnect.Application.Mappings;
using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using Mediator;

namespace CrisisConnect.Application.UseCases.Transactions.EnvoyerMessage;

public class EnvoyerMessageCommandHandler : IRequestHandler<EnvoyerMessageCommand, MessageDto>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly AppMapper _mapper;

    public EnvoyerMessageCommandHandler(ITransactionRepository transactionRepository, AppMapper mapper)
    {
        _transactionRepository = transactionRepository;
        _mapper = mapper;
    }

    public async ValueTask<MessageDto> Handle(EnvoyerMessageCommand request, CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.GetByIdAsync(request.TransactionId, cancellationToken)
            ?? throw new NotFoundException(nameof(Transaction), request.TransactionId);

        var message = transaction.Discussion.AjouterMessage(
            request.ExpediteurId, request.Contenu, request.Langue);

        await _transactionRepository.UpdateAsync(transaction, cancellationToken);

        return _mapper.ToDto(message);
    }
}
