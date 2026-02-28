using CrisisConnect.Domain.Enums;

namespace CrisisConnect.Application.DTOs;

public record NotificationDto(
    Guid Id,
    Guid DestinataireId,
    TypeNotification Type,
    string Contenu,
    bool EstLue,
    DateTime DateCreation);
