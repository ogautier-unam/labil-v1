namespace CrisisConnect.Web.Models;

public record AuthResponseModel(
    Guid UserId,
    string Email,
    string Role,
    string AccessToken,
    string RefreshToken);
