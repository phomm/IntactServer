using System.ComponentModel.DataAnnotations;

namespace Intact.BusinessLogic.Data.Models;

public record FactionDao: LocalizableDao
{
    public int Number { get; init; }
}