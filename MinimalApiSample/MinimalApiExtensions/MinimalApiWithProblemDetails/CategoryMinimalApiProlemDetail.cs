using MinimalApiSample.Models;

namespace MinimalApiSample.MinimalApiExtensions
{
	/// <summary>
	/// Using Problem Details and ValidationProblem Details in Results and
	/// their conterparts in TypedResults for generate consistent result
	/// response for all minimal API's
	/// 
	/// Problem Details by default return 500 status code
	/// ValidationProblem Details by default return 400 status code
	/// </summary>
	public static class CategoryMinimalApiProlemDetail
	{
		public static WebApplication AllCategoriesProblemResult(this WebApplication app)
		{
			app.MapGet("/Category", (CategoryRepositoy repository) =>
			{
				IReadOnlyCollection<Category> categories = repository.Categories;
				return TypedResults.Ok(categories);
			});

			return app;
		}


		public static WebApplication FindByIdProblemResult(this WebApplication app)
		{
			app.MapGet("/Category/{id:int?}", (int? id, CategoryRepositoy repository) =>
			{
				if (id is null or <= 0) return Results.Problem(
					detail: "Category id is null",
					statusCode: StatusCodes.Status404NotFound,
					title: "Category Not Found");

				Category? category = repository.Find(id ?? 0);
				if (category is null) return Results.Problem(
					detail: $"Category not found with id: {id}",
					statusCode: StatusCodes.Status404NotFound,
					title: "Category Not Found");

				return TypedResults.Ok(category);
			});
			return app;
		}


		public static WebApplication AddProblemResult(this WebApplication app)
		{
			app.MapPost("/Category", (Category model, CategoryRepositoy repository) =>
			{
				if (string.IsNullOrEmpty((string)model.Title))
				{
					return Results.ValidationProblem(
								new Dictionary<string, string[]>()
								{
									{ "ValidationErrors", ["Category Title is null"] }
								},
								title: "Category is invlaid",
								detail: "Cattegory Validation Errors",
								statusCode: StatusCodes.Status400BadRequest);
				}

				Category? category = repository.Add(model);
				return category is not null
						? TypedResults.Ok(category)
						: Results.Problem(
							title: "Error in adding category",
							statusCode: StatusCodes.Status400BadRequest,
							detail: "Error in adding new category");
			});

			return app;
		}



		public static WebApplication DeleteProblemResult(this WebApplication app)
		{
			app.MapDelete("/Category/{id:int?}", (int? id, CategoryRepositoy repository) =>
			{
				if (id is null or <= 0)
					return Results.Problem(
						title: "Category Not Found",
						statusCode: StatusCodes.Status404NotFound,
						detail: "category id null");

				Category? deletedCategory = repository.Delete(id ?? 0);
				return deletedCategory is not null
						? TypedResults.Ok(deletedCategory)
						: Results.Problem(
							title: "Category Not Found",
							statusCode: StatusCodes.Status404NotFound,
							detail: $"category not exist with id: {id}");
			});

			return app;
		}


		public static WebApplication EditProblemResult(this WebApplication app)
		{
			app.MapPut("/Category", (Category model, CategoryRepositoy repository) =>
			{
				Dictionary<string, string[]> validationErrors = [];
				List<string> validationMessages = [];

				if (string.IsNullOrWhiteSpace(model.Title) || string.IsNullOrEmpty(model.Title))
					validationMessages.Add("Category Title can not be empty or null");
				if (model.Title.Length < 2 && model.Title.Length > 100)
					validationMessages.Add("Category Title must had len in range 2-100");
				if (model.Id == 0)
					validationMessages.Add("Category Identifier is invalid");

				if (validationMessages.Count > 0)
				{
					validationErrors.TryAdd("ValidationErrors", [.. validationMessages]);
					return Results.ValidationProblem(
						validationErrors,
						title: "Category is invalid",
						statusCode: StatusCodes.Status400BadRequest,
						detail: "Please check model validation errors");
				}

				Category? category = repository.Update(model);
				return category is not null
					? TypedResults.Ok(category)
					: Results.Problem(
						title: "Error in category update",
						statusCode: StatusCodes.Status400BadRequest,
						detail: "for more information check your log please");
			});

			return app;
		}
	}
}
