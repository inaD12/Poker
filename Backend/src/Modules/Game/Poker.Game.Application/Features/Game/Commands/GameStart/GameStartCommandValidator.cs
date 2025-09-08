using FluentValidation;
using Poker.Game.Domain.Utilities;

namespace Poker.Game.Application.Features.Game.Commands.GameStart;

internal class GameStartCommandValidator : AbstractValidator<GameStartCommand>
{
    public GameStartCommandValidator()
    {
        RuleFor(x => x.LobbyId)
            .NotEmpty()
            .MinimumLength(GameBusinessConfiguration.ID_MIN_LENGTH)
            .MaximumLength(GameBusinessConfiguration.ID_MAX_LENGTH);
    }
}