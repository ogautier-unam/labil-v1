using AutoMapper;
using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using MediatR;

namespace CrisisConnect.Application.UseCases.Transactions.InitierTransaction;

public class InitierTransactionCommandHandler : IRequestHandler<InitierTransactionCommand, TransactionDto>
{
    private readonly IPropositionRepository _propositionRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IMapper _mapper;

    public InitierTransactionCommandHandler(
        IPropositionRepository propositionRepository,
        ITransactionRepository transactionRepository,
        IMapper mapper)
    {
        _propositionRepository = propositionRepository;
        _transactionRepository = transactionRepository;
        _mapper = mapper;
    }

    public async Task<TransactionDto> Handle(InitierTransactionCommand request, CancellationToken cancellationToken)
    {
        var proposition = await _propositionRepository.GetByIdAsync(request.PropositionId, cancellationToken)
            ?? throw new NotFoundException(nameof(Proposition), request.PropositionId);

        proposition.MarquerEnTransaction();

        var transaction = new Transaction(request.PropositionId, request.InitiateurId);
        await _transactionRepository.AddAsync(transaction, cancellationToken);

        return _mapper.Map<TransactionDto>(transaction);
    }
}
