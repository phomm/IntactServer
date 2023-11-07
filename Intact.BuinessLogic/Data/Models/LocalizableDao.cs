using System.ComponentModel.DataAnnotations;

namespace Intact.BusinessLogic.Data.Models;

public record LocalizableDao
{
    [StringLength(32)]
    public string TermName { get; set; }
    [StringLength(32)]
    public string TermDescription { get; set; }
}