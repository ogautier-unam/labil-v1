using CrisisConnect.Application.DTOs;
using CrisisConnect.Application.Mappings;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using Mediator;

namespace CrisisConnect.Application.UseCases.Propositions.RefuserValidationProposition;

public class RefuserValidationPropositionCommandHandler
    : IRequestHandler<RefuserValidationPropositionCommand, PropositionAvecValidationDto>
{
    private readonly IPropositionRepository _repository;
    private readonly AppMapper _mapper;

    public RefuserValidationPropositionCommandHandler(IPropositionRepository repository, AppMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async ValueTask<PropositionAvecValidationDto> Handle(
        RefuserValidationPropositionCommand request, CancellationToken cancellationToken)
    {
        var proposition = await _repository.GetByIdAsync(request.Id, cancellationToken) as PropositionAvecValidation
            ?? throw new NotFoundException(nameof(PropositionAvecValidation), request.Id);

        proposition.RefuserValidation(request.ValideurEntiteId);
        await _repository.UpdateAsync(proposition, cancellationToken);

        return _mapper.ToDto(proposition);
    }
}
