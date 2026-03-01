namespace CrisisConnect.Web.Models;

public record MethodeIdentificationModel(
    Guid Id,
    Guid PersonneId,
    string TypeMethode,
    string NiveauFiabilite,
    bool EstVerifiee,
    DateTime? DateVerification);
