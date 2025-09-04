using System.ComponentModel.DataAnnotations;

namespace Poker.Users.Presentation.Features.Models.Requests;

public class UpdateCurrentUserRequest
{
    [Required] public string NewUsername { get; set; }
}