namespace CrisisConnect.Application.DTOs;

public record MessageDto(
    Guid Id,
    Guid DiscussionId,
    Guid ExpediteurId,
    string Contenu,
    string Langue,
    bool IssueTraductionAuto,
    DateTime DateEnvoi);
