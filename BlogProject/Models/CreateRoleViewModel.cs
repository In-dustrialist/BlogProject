using System.ComponentModel.DataAnnotations;

namespace BlogProject.Models
{
    public class CreateRoleViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название роли обязательно")]
        [StringLength(50)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Описание обязательно")]
        [StringLength(200)]
        public string Description { get; set; }
    }
}
