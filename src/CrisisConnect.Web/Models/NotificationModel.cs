namespace CrisisConnect.Web.Models;

public record NotificationModel(
    Guid Id,
    Guid DestinataireId,
    string Type,
    string Contenu,
    bool EstLue,
    DateTime DateCreation);
