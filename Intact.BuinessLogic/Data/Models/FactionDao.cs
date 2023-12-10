using System.ComponentModel.DataAnnotations;

namespace Intact.BusinessLogic.Data.Models;

public record FactionDao: LocalizableDao
{
    [Key]
    [StringLength(16)]
    public string Id { get; init; }
    public int Number { get; init; }
}