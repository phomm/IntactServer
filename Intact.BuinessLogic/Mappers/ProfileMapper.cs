using Intact.BusinessLogic.Data.Models;
using Intact.BusinessLogic.Models;
using Riok.Mapperly.Abstractions;

namespace Intact.BusinessLogic.Mappers;

[Mapper]
public partial class ProfileMapper
{
    public partial Profile Map(ProfileDao dao);

    public static IReadOnlyList<Profile> Map(IReadOnlyList<ProfileDao> daos)
    {
        var mapper = new ProfileMapper();
        return daos.Select(mapper.Map).OrderBy(x => x.Name).ToList();
    }
}