namespace CrisisConnect.Application.DTOs;

public record MethodeIdentificationDto(
    Guid Id,
    Guid PersonneId,
    string TypeMethode,
    string NiveauFiabilite,
    bool EstVerifiee,
    DateTime DateVerification);
