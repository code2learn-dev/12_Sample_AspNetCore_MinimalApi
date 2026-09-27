using MinimalApiSample.Models;
using System.Net;

namespace MinimalApiSample.MinimalApiExtensions.MinimalApiWithResult
{
    /// <summary>
    /// Add Minimal Apis that these return specified result status code
    /// </summary>
    public static class CategoryResultMinimalApiExtensions
    {
        public static WebApplication GetCategiriesResult(this WebApplication app)
        {
            app.MapGet("/Category", (CategoryRepositoy repository) =>
            {
                IReadOnlyCollection<Category> categories = repository.Categories;
                return TypedResults.Ok(categories);
            }).Produces(StatusCodes.Status200OK, responseType: typeof(IReadOnlyCollection<Category>));

            return app;
        }


        public static WebApplication AddCategoryResult(this WebApplication app)
        {
            app.MapPost("/Caregory", (Category model, CategoryRepositoy repository) =>
            {
                Category? category = repository.Add(model);
                return category is not null
                        ? TypedResults.Ok(category)
                        : Results.BadRequest("Error in add category");
            })
                .Produces(StatusCodes.Status200OK, responseType: typeof(Category))
                .Produces(StatusCodes.Status400BadRequest, responseType: typeof(string));
            return app;
        }


        public static WebApplication FindCategoryResult(this WebApplication app)
        {
            app.MapGet("/Category/{id:int?}", (int? id, CategoryRepositoy repository) =>
            {
                if (id is null or <= 0) return Results.BadRequest("category id is null");
                Category? category = repository.Find(id ?? 0);
                return category is not null
                        ? TypedResults.Ok(category)
                        : Results.NotFound("category not found");
            })
                .Produces(StatusCodes.Status200OK, responseType: typeof(Category))
                .Produces(StatusCodes.Status400BadRequest, responseType: typeof(string))
                .Produces(StatusCodes.Status404NotFound, responseType: typeof(string));
            return app;
        }


        public static WebApplication UpdateCategoryResult(this WebApplication app)
        {
            app.MapPut("/Category", (Category model, CategoryRepositoy repository) =>
            {
                Category? updatedCategory = repository.Update(model);
                return updatedCategory is not null
                        ? TypedResults.Ok(updatedCategory)
                        : Results.BadRequest("Error in updating category");
            })
                .Produces(StatusCodes.Status200OK, responseType: typeof(Category))
                .Produces(StatusCodes.Status400BadRequest, responseType: typeof(string));

            return app;
        }
    }
}
