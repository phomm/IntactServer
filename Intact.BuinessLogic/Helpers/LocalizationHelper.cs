using Intact.BusinessLogic.Data.Models;
using Intact.BusinessLogic.Models;

namespace Intact.BusinessLogic.Helpers;

public static class LocalizationExtensions
{
    public static Dictionary<string, string> Map(this IEnumerable<IGrouping<string , LocalizationDao>> localizations, string termName, string defaultValue) => 
        localizations.FirstOrDefault(x => x.Key == termName)?.ToDictionary(x => x.LanguageCode, x => x.Value) ?? new Dictionary<string, string> { { "English", defaultValue } };

    public static void SetupLocalization(this Localizable localizable, LocalizableDao localizableDao, IReadOnlyList<IGrouping<string, LocalizationDao>> localizations)
    {
        localizable.Name = localizations.Map(localizableDao.TermName, localizable.Id);
        localizable.Description = localizations.Map(localizableDao.TermDescription, localizable.Id);
    }
}