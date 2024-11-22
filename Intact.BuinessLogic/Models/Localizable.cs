using Intact.BusinessLogic.Data.Models;
using Intact.BusinessLogic.Helpers;

namespace Intact.BusinessLogic.Models;

public record Localizable
{
    public string Id { get; init; }
    public IReadOnlyDictionary<string, string> Name { get; set; }
    public IReadOnlyDictionary<string, string> Description { get; set; }
}