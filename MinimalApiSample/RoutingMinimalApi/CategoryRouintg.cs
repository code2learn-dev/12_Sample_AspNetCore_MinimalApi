using MinimalApiSample.Models;

namespace MinimalApiSample.RoutingMinimalApi
{
    public static class CategoryRouintg
    {
        public static WebApplication FindCategoryByIdRouting(this WebApplication app)
        {
            // with optional route parameter can match with -12, 0 , 12
            //app.MapGet("/Category/{id:int?}", (int? id, CategoryRepositoy repository) =>

            // with define constraint in a route template for a given route parameter id
            // the URLs that contain numbers just greater than or equal 1 is match with 
            // the endpoint route template and any other number like -12 or 0 not match with
            // endpoint route template and then cause 404 status code in response
            app.MapGet("/Category/{id:int:min(1)}", (int id, CategoryRepositoy repository) =>
            { 
                Category? category = repository.Find(id);
                return category is not null
                    ? TypedResults.Ok(category)
                    : Results.Problem(
                                      title: "Category Not Found",
                                      statusCode: StatusCodes.Status404NotFound,
                                      detail: $"Category with id: {id} not found");
            });

            return app;
        }
    }
}
