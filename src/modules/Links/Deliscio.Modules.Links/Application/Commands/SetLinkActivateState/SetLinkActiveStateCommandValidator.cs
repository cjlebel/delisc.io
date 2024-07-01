using FluentValidation;

namespace Deliscio.Modules.Links.Application.Commands.SetLinkActivateState;

internal class SetLinkActiveStateCommandValidator : AbstractValidator<SetLinkActiveStateCommand>
{
    public SetLinkActiveStateCommandValidator()
    {
        RuleFor(x => x.LinkId).NotEmpty()
            .WithMessage("Link LinkId is required");

        RuleFor(x => x.UpdateByUserId).NotEmpty()
            .WithMessage("User LinkId is required");
    }
}