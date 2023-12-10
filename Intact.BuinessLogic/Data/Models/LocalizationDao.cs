using System.ComponentModel.DataAnnotations;

namespace Intact.BusinessLogic.Data.Models;

public record LocalizationDao
{
    [Key]
    [StringLength(32)]
    public string TermId { get; init; }
    [Key]
    [StringLength(16)]
    public string LanguageCode { get; init; }
    [StringLength(950)]
    public string Value { get; init; }
}