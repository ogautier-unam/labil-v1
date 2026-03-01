namespace CrisisConnect.Web.Models;

public record MessageModel(
    Guid Id,
    Guid DiscussionId,
    Guid ExpediteurId,
    string Contenu,
    string Langue,
    bool IssueTraductionAuto,
    DateTime DateEnvoi);
