using Intact.BusinessLogic.Data.Models;
using Intact.BusinessLogic.Helpers;

namespace Intact.BusinessLogic.Models;

public record Localizable
{
    public string Id { get; init; }
    public IReadOnlyDictionary<string, string> Name { get; set; }
    public IReadOnlyDictionary<string, string> Description { get; set; }
    public void SetupLocalization(LocalizableDao localizableDao, IReadOnlyList<IGrouping<string, LocalizationDao>> localizations)
    {
        Name = localizations.Map(localizableDao.TermName, Id);
        Description = localizations.Map(localizableDao.TermDescription, Id);
    }
}