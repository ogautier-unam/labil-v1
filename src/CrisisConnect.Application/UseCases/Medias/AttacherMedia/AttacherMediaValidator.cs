using FluentValidation;

namespace CrisisConnect.Application.UseCases.Medias.AttacherMedia;

public class AttacherMediaValidator : AbstractValidator<AttacherMediaCommand>
{
    public AttacherMediaValidator()
    {
        RuleFor(x => x.PropositionId).NotEmpty();
        RuleFor(x => x.Url).NotEmpty().MaximumLength(500);
    }
}
