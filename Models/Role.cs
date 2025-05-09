using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BlogProject.Models
{
    public class Role : IdentityRole
    {
        [Display(Name = "Описание")]
        [MaxLength(250, ErrorMessage = "Описание не должно превышать 250 символов")]
        public string Description { get; set; }
    }
}
