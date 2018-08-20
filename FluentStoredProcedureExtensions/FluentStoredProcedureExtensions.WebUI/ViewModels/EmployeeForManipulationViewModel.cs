using System.ComponentModel.DataAnnotations;

namespace FluentStoredProcedureExtensions.WebUI.ViewModels
{
    public abstract class EmployeeForManipulationViewModel
    {
        [Required]
        [MaxLength(155)]
        public virtual string Name { get; set; }

        [Required]
        [MaxLength(255)]
        [EmailAddress]
        public virtual string Email { get; set; }
    }
}
