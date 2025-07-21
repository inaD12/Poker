using FluentValidation;
using Poker.Game.Domain.Utilities;

namespace Poker.Game.Application.Features.Lobby.Commands.AddPlayerToLobby;

public class AddPlayerToLobbyCommandValidator : AbstractValidator<AddPlayerToLobbyCommand>
{
    public AddPlayerToLobbyCommandValidator()
    {
        RuleFor(x => x.PlayerId)
            .NotEmpty()
            .MinimumLength(GameBusinessConfiguration.ID_MIN_LENGTH)
            .MaximumLength(GameBusinessConfiguration.ID_MAX_LENGTH);

        RuleFor(x => x.LobbyId)
            .NotEmpty()
            .MinimumLength(GameBusinessConfiguration.ID_MIN_LENGTH)
            .MaximumLength(GameBusinessConfiguration.ID_MAX_LENGTH);
    }
}