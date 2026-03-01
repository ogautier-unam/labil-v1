using CrisisConnect.Application.Mappings;
using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using Mediator;

namespace CrisisConnect.Application.UseCases.Transactions.GetDiscussion;

public class GetDiscussionQueryHandler : IRequestHandler<GetDiscussionQuery, DiscussionDto>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly AppMapper _mapper;

    public GetDiscussionQueryHandler(ITransactionRepository transactionRepository, AppMapper mapper)
    {
        _transactionRepository = transactionRepository;
        _mapper = mapper;
    }

    public async ValueTask<DiscussionDto> Handle(GetDiscussionQuery request, CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.GetByIdAsync(request.TransactionId, cancellationToken)
            ?? throw new NotFoundException(nameof(Transaction), request.TransactionId);

        // TransactionId n'est pas directement sur Discussion — on le transmet via le mapping
        var dto = new DiscussionDto(
            transaction.Discussion.Id,
            transaction.Id,
            transaction.Discussion.Visibilite.ToString(),
            _mapper.ToDto(transaction.Discussion.Messages.OrderBy(m => m.DateEnvoi).ToList()));

        return dto;
    }
}
