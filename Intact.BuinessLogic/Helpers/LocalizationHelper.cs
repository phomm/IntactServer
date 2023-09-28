using Intact.BusinessLogic.Data.Models;

namespace Intact.BusinessLogic.Helpers;

public static class LocalizationExtensions
{
    public static Dictionary<string, string> Map(this IEnumerable<IGrouping<string , LocalizationDao>> localizations, string termName, string defaultValue) => 
        localizations.FirstOrDefault(x => x.Key == termName)?.ToDictionary(x => x.LanguageCode, x => x.Value) ?? new Dictionary<string, string> { { "English", defaultValue } };
}