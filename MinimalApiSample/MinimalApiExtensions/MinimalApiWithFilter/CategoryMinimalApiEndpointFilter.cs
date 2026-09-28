using MinimalApiSample.Filters;
using MinimalApiSample.Models;

namespace MinimalApiSample.MinimalApiExtensions.MinimalApiWithFilter
{
    public static class CategoryMinimalApiEndpointFilter
    {
        public static WebApplication FindCategoryEndpointFilter(this WebApplication app)
        {
            app.MapGet("/Category/{id:int}", (int id, CategoryRepositoy repository) =>
            {
                Category? category = repository.Find(id);
                return category is not null
                    ? TypedResults.Ok(category)
                    : Results.Problem(
                                      title: "Category Not Found",
                                      statusCode: StatusCodes.Status404NotFound,
                                      detail: $"Category with id: {id} not found");
            }).AddEndpointFilter<CategoryIdValidationFilter>();

            return app;
        }
    }
}
