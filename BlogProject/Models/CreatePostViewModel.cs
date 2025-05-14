using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogProject.Models
{
    public class CreatePostViewModel
    {
        [Required(ErrorMessage = "Введите заголовок")]
        [StringLength(100, ErrorMessage = "Заголовок не должен превышать 100 символов")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Введите краткое описание")]
        [StringLength(300, ErrorMessage = "Описание не должно превышать 300 символов")]
        public string Summary { get; set; }

        [Required(ErrorMessage = "Введите содержание")]
        public string Content { get; set; }

        public IEnumerable<Tag> AvailableTags { get; set; }

        public List<int> SelectedTags { get; set; } = new List<int>();
    }
}
