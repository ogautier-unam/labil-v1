using FluentValidation;

namespace CrisisConnect.Application.UseCases.Missions.CreateMission;

public class CreateMissionValidator : AbstractValidator<CreateMissionCommand>
{
    public CreateMissionValidator()
    {
        RuleFor(x => x.Titre).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.PropositionId).NotEmpty();
        RuleFor(x => x.CreePar).NotEmpty();
        RuleFor(x => x.NombreBenevoles).GreaterThan(0);
    }
}
