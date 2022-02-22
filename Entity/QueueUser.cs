using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace hospital.Entity;

public class QueueUser : IdentityUser
{
    [Required(ErrorMessage = "Enter the fullname")]
    [Display(Name = "Fullname")]
    public string Fullname { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset ModifiedAt { get; set; }
    public DateTimeOffset Birthdate { get; set; }
    public bool IsActive { get; set; }
}