#pragma warning disable 8618

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Intact.BusinessLogic.Data.Models;

public record LocalizationDao
{
    [Key]
    [StringLength(32)]
    public string TermId { get; set; }
    [Key]
    [StringLength(16)]
    public string LanguageCode { get; set; }
    [StringLength(950)]
    public string Value { get; set; }
}