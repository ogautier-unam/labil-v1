namespace CrisisConnect.Domain.Entities;

public class Message
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid DiscussionId { get; private set; }
    public Guid ExpediteurId { get; private set; }
    public string Contenu { get; private set; } = string.Empty;
    public string Langue { get; private set; } = "fr";
    public bool IssueTraductionAuto { get; private set; }
    public string? TexteOriginal { get; private set; }
    public DateTime DateEnvoi { get; private set; } = DateTime.UtcNow;

    protected Message() { }

    public Message(Guid discussionId, Guid expediteurId, string contenu, string langue = "fr",
        bool issueTraductionAuto = false, string? texteOriginal = null)
    {
        DiscussionId = discussionId;
        ExpediteurId = expediteurId;
        Contenu = contenu;
        Langue = langue;
        IssueTraductionAuto = issueTraductionAuto;
        TexteOriginal = texteOriginal;
    }
}
