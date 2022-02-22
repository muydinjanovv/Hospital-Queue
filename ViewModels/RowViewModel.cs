using System.ComponentModel.DataAnnotations;

namespace hospital.ViewModels;

public class RowViewModel
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Required(ErrorMessage = "Enter the fullname")]
    [Display(Name = "Fullname")]
    public string Fullname { get; set; }

    [Required(ErrorMessage = "Enter the telephone number")]
    [RegularExpression(@"^[\+]?(998[-\s\.]?)([0-9]{2}[-\s\.]?)([0-9]{3}[-\s\.]?)([0-9]{2}[-\s\.]?)([0-9]{2}[-\s\.]?)$",
    ErrorMessage = "Format of telephone number is not correct.")]
    [Display(Name = "Telephone number")]
    public string Phone { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset ModifiedAt { get; set; }
    public bool IsActive { get; set; }
}