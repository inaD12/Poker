using Poker.Common.Presentation.Models;

namespace Poker.Common.Presentation.Abstractions;

public interface IClaimsExtractor
{
    string GetUserId();
    ClaimsExtractorModel GetAllClaims();
    string GetUserId(string token);
}