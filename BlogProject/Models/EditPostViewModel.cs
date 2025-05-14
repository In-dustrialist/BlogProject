using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogProject.Models
{
    public class EditPostViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите заголовок")]
        [StringLength(100)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Введите краткое описание")]
        [StringLength(300)]
        public string Summary { get; set; }

        [Required(ErrorMessage = "Введите содержание")]
        public string Content { get; set; }

        public List<int> SelectedTags { get; set; } = new List<int>();

        public List<Tag> AvailableTags { get; set; } = new List<Tag>();
    }
}
