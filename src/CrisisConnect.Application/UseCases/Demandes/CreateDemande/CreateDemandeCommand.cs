using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Enums;
using Mediator;

namespace CrisisConnect.Application.UseCases.Demandes.CreateDemande;

public record CreateDemandeCommand(
    string Titre,
    string Description,
    Guid CreePar,
    OperateurLogique OperateurLogique = OperateurLogique.Simple,
    NiveauUrgence Urgence = NiveauUrgence.Moyen,
    string? RegionSeverite = null,
    double? Latitude = null,
    double? Longitude = null,
    bool EstRecurrente = false,
    string? FrequenceRecurrence = null) : IRequest<DemandeDto>;
