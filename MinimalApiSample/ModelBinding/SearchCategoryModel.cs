using Microsoft.AspNetCore.Mvc;

namespace MinimalApiSample.ModelBinding
{
    public record SearchCategoryModel(
        int id,
        int title,
        [FromQuery] string page,
        [FromHeader(Name = "order")] string order)
    {
    }
}
