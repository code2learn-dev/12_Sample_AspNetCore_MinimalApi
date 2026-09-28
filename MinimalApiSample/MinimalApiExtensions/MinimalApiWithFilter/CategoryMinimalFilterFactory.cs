using MinimalApiSample.Filters;
using MinimalApiSample.Models;

namespace MinimalApiSample.MinimalApiExtensions.MinimalApiWithFilter
{
    public static class CategoryMinimalFilterFactory
    {
        public static WebApplication FindCategoryWithFilterFactory(this WebApplication app)
        {
            app.MapGet("/Category/{id:int?}", (CategoryRepositoy repository, int? id) =>
            {
                Category? category = repository.Find(id ?? 0);
                return category is not null
                    ? TypedResults.Ok(category)
                    : Results.Problem(
                                      title: "Category Not Found",
                                      statusCode: StatusCodes.Status404NotFound,
                                      detail: $"Category not found with id: {id}");
            }).AddEndpointFilterFactory(CategoryFilterFactory.ValidateCategoryModelContainsId);

            return app;
        }


        public static WebApplication DeleteCategoryWithFilterFactory(this WebApplication app)
        {
            app.MapDelete("/Category/{id:int?}", (int? id, CategoryRepositoy repository) =>
            {
                Category? category = repository.Find(id ?? 0);
                if (category is null)
                    return Results.Problem(
                                        title: "Category Not Found",
                                        statusCode: StatusCodes.Status404NotFound,
                                        detail: $"Category with id: {id} not found");

                Category? deletedCategory = repository.Delete(id ?? 0);
                return deletedCategory is not null
                    ? TypedResults.Ok(deletedCategory)
                    : Results.Problem(
                                    title: "Error in deleteing category",
                                    statusCode: StatusCodes.Status400BadRequest,
                                    detail: "for more information please check logg errir");
            }).AddEndpointFilterFactory(CategoryFilterFactory.ValidateCategoryModelContainsId);
            return app;
        }


        public static WebApplication EditCategoryWIthFilterFactory(this WebApplication app)
        {
            app.MapPut("/Category/{id:int?}", (Category model, int? id, CategoryRepositoy repository) =>
            {
                Category? category = repository.Find(id ?? 0);
                if (category is null)
                    return Results.Problem(
                                        title: "Error Category Update",
                                        statusCode: StatusCodes.Status404NotFound,
                                        detail: "Category Not Found to update");

                Category? updatedCategory = repository.Update(model);
                return updatedCategory is not null
                        ? TypedResults.Ok(updatedCategory)
                        : Results.Problem(
                                        title: "Error in category update",
                                        statusCode: StatusCodes.Status400BadRequest,
                                        detail: "for more information please visit the log file");
            }).AddEndpointFilterFactory(CategoryFilterFactory.ValidateCategoryModel);

            return app;
        }
    }
}
