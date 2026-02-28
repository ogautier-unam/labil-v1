using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Enums;

namespace CrisisConnect.Domain.Tests;

public class MediaTests
{
    [Fact]
    public void NouvelleInstance_PropositionIdUrlType_Setés()
    {
        var propositionId = Guid.NewGuid();
        var media = new Media(propositionId, "https://cdn.example.com/photo.jpg", TypeMedia.Photo);

        Assert.Equal(propositionId, media.PropositionId);
        Assert.Equal("https://cdn.example.com/photo.jpg", media.Url);
        Assert.Equal(TypeMedia.Photo, media.Type);
    }

    [Fact]
    public void NouvelleInstance_PourChaquType_TypeCorrect()
    {
        var pid = Guid.NewGuid();

        Assert.Equal(TypeMedia.Video, new Media(pid, "v.mp4", TypeMedia.Video).Type);
        Assert.Equal(TypeMedia.Audio, new Media(pid, "a.mp3", TypeMedia.Audio).Type);
        Assert.Equal(TypeMedia.Document, new Media(pid, "d.pdf", TypeMedia.Document).Type);
    }

    [Fact]
    public void NouvelleInstance_DateAjout_EstRécenteUTC()
    {
        var avant = DateTime.UtcNow.AddSeconds(-1);
        var media = new Media(Guid.NewGuid(), "https://cdn.example.com/img.png", TypeMedia.Photo);
        var après = DateTime.UtcNow.AddSeconds(1);

        Assert.InRange(media.DateAjout, avant, après);
    }
}
