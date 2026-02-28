using AutoMapper;
using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Interfaces.Repositories;
using MediatR;

namespace CrisisConnect.Application.UseCases.Transactions.GetTransactions;

public class GetTransactionsQueryHandler : IRequestHandler<GetTransactionsQuery, IReadOnlyList<TransactionDto>>
{
    private readonly ITransactionRepository _repository;
    private readonly IMapper _mapper;

    public GetTransactionsQueryHandler(ITransactionRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<TransactionDto>> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
    {
        var transactions = await _repository.GetAllAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<TransactionDto>>(transactions);
    }
}
