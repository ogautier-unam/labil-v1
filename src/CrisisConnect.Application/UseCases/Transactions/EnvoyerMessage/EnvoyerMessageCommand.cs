using CrisisConnect.Application.DTOs;
using Mediator;

namespace CrisisConnect.Application.UseCases.Transactions.EnvoyerMessage;

public record EnvoyerMessageCommand(
    Guid TransactionId,
    Guid ExpediteurId,
    string Contenu,
    string Langue = "fr")
    : IRequest<MessageDto>;
