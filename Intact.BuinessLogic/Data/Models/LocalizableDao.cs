using System.ComponentModel.DataAnnotations;

namespace Intact.BusinessLogic.Data.Models;

public record LocalizableDao
{
    [Key]
    [StringLength(16)]
    public string Id { get; init; }
    [StringLength(32)]
    public string TermName { get; init; }
    [StringLength(32)]
    public string TermDescription { get; init; }
}