using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using MediatR;

namespace CrisisConnect.Application.UseCases.Taxonomie.DesactiverCategorie;

public class DesactiverCategorieCommandHandler : IRequestHandler<DesactiverCategorieCommand>
{
    private readonly ICategorieTaxonomieRepository _repository;

    public DesactiverCategorieCommandHandler(ICategorieTaxonomieRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DesactiverCategorieCommand request, CancellationToken cancellationToken)
    {
        var categorie = await _repository.GetByIdAsync(request.CategorieId, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.CategorieTaxonomie), request.CategorieId);

        categorie.Desactiver();
        await _repository.UpdateAsync(categorie, cancellationToken);
    }
}
