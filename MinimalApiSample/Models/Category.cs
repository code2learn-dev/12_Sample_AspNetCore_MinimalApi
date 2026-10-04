using System.ComponentModel.DataAnnotations;

namespace MinimalApiSample.Models
{
    public class Category
    {
        [Range(1, 1_000_000, ErrorMessage = "شناسه دسته آموزشی باید بزرگتر از یک باشد")]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "عنوان دسته آموزشی را وارد کنید")]
        public string Title { get; set; } = string.Empty;
    }
}
