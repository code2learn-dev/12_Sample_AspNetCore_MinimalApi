using System.ComponentModel.DataAnnotations;

namespace MinimalApiSample.ModelBinding
{
    public record CategoryIdValidation
    {
        [Required(ErrorMessage = "شناسه محصول را وارد کنید")]
		[Range(1, 1_000_000, ErrorMessage = "شناسه دسته آموزشی باید بزرگتر از یک باشد")]
		public int Id { get; set; }
    }
}
