using MinimalApiSample.Filters;
using MinimalApiSample.Models;

namespace MinimalApiSample.MinimalApiExtensions.MinimalApiWithFilter
{
    public static class CategoryMinimalApiFilter
    {
        public static WebApplication FindCategoryValidateFilter(this WebApplication app)
        {
            app.MapGet("/Category/{id:int?}", static (int? id, CategoryRepositoy repository) =>
            {
                Category? category = repository.Find(id ?? 0);
                return category is not null
                    ? TypedResults.Ok(category)
                    : Results.Problem(
                        title: "Category Not Foutnd",
                        statusCode: StatusCodes.Status404NotFound,
                        detail: $"Category not found with id: {id}");
            }).AddEndpointFilter(SimpleValidationFilters.ValidateCategoryId);

            return app;
        }


        public static WebApplication AddCategoryValidationFilter(this WebApplication app)
        {
            app.MapPost("/Category", (Category model, CategoryRepositoy repository) =>
            {
                Category? category = repository.Add(model);
                return category is not null
                    ? TypedResults.Ok(category)
                    : Results.Problem(
                        title: "Error in adding new Category",
                        statusCode: StatusCodes.Status400BadRequest,
                        detail: "for more information Please check logging error");
            }).AddEndpointFilter(SimpleValidationFilters.ValidateCategoryModel);

            return app;
        }
    }
}
