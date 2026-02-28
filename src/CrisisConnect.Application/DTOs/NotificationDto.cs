namespace CrisisConnect.Application.DTOs;

public record NotificationDto(
    Guid Id,
    Guid DestinataireId,
    string Sujet,
    string Contenu,
    bool EstLue,
    DateTime CreeLe);
