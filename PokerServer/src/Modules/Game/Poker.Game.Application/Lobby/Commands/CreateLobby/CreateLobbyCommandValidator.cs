using FluentValidation;
using Poker.Game.Domain.Utilities;

namespace Poker.Game.Application.Lobby.Commands.CreateLobby;

public class CreateLobbyCommandValidator: AbstractValidator<CreateLobbyCommand>
{
    public CreateLobbyCommandValidator()
    {
        RuleFor(x => x.StartingPlayerId)
            .NotEmpty()
            .MinimumLength(GameBusinessConfiguration.ID_MIN_LENGTH)
            .MaximumLength(GameBusinessConfiguration.ID_MAX_LENGTH);
    }
}