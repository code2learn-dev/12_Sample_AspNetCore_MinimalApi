using MinimalApiSample.Models;
using System.Net.Mime;

namespace MinimalApiSample.MinimalApiExtensions.SimpleMinimalApi
{
    public static class CategoryMinimalApiExtensions
    {
		public static WebApplication GetCategoriesApi(this WebApplication app)
		{
			app.MapGet("/category", (CategoryRepositoy repository) => repository.Categories);
			return app;
		}

		public static WebApplication FindCategory(this WebApplication app)
		{
			app.MapGet("/Category/{id:int}", (int? id, CategoryRepositoy repository) =>
				repository.Find(id ?? 0));

			return app;
		}

		public static WebApplication AddCategory(this WebApplication app)
		{
			app.MapPost("/category/add", (Category category, CategoryRepositoy repository) =>
				repository.Add(category));

			return app;
		}

		public static WebApplication EditCategory(this WebApplication app)
		{
			app.MapPut("/Category", (Category category, CategoryRepositoy repository) =>
				repository.Update(category));

			return app;
		}
	}
}
