using CrisisConnect.Application.UseCases.Notifications.MarkAsRead;

namespace CrisisConnect.Application.Tests;

public class MarkNotificationAsReadValidatorTests
{
    private readonly MarkNotificationAsReadValidator _validator = new();

    [Fact]
    public async Task Valide_NotificationIdRempli_PasseValidation()
    {
        var cmd = new MarkNotificationAsReadCommand(Guid.NewGuid());
        var result = await _validator.ValidateAsync(cmd);
        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task Invalide_NotificationIdVide_EchecSurNotificationId()
    {
        var cmd = new MarkNotificationAsReadCommand(Guid.Empty);
        var result = await _validator.ValidateAsync(cmd);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "NotificationId");
    }
}
