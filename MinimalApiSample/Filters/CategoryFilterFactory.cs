using MinimalApiSample.Models;
using System.Numerics;
using System.Reflection;

namespace MinimalApiSample.Filters
{
	public class CategoryFilterFactory
	{
		public static EndpointFilterDelegate ValidateCategoryModelContainsId(
			EndpointFilterFactoryContext context,
			EndpointFilterDelegate next)
		{
			int? idPosition = null;
			ParameterInfo[] parameters = context.MethodInfo.GetParameters();
			for (int i = 0; i < parameters.Length; i++)
			{
				if (parameters[i]?.Name?
					.Equals("id", StringComparison.InvariantCultureIgnoreCase) ?? false
					&& parameters[i].ParameterType == typeof(int))
				{
					idPosition = i;
					break;
				}
			}

			if (!idPosition.HasValue) return next;

			return async (invocationContext) =>
			{
				int? id = invocationContext.GetArgument<int?>(idPosition.Value);
				if (id is null or <= 0)
				{
					return Results.ValidationProblem(
						new Dictionary<string, string[]>
						{
							{ "ValidationErrors", ["Category Id is invalid"]}
						},
						title: "Category Not Found",
						statusCode: StatusCodes.Status400BadRequest,
						detail: $"Category id is null or less than 0");
				}

				return await next(invocationContext);
			};
		}


		public static EndpointFilterDelegate ValidateCategoryModel(
			EndpointFilterFactoryContext context,
			EndpointFilterDelegate next)
		{
			int? idPosition = null;
			int? modelPosition = null;
			ParameterInfo[] parameters = context.MethodInfo.GetParameters();
			for (int i = 0; i < parameters.Length; i++)
			{
				if (parameters?[i].Name?
					.Equals("id", StringComparison.InvariantCultureIgnoreCase) ?? false
					&& parameters[i].ParameterType == typeof(int))
				{
					idPosition = i;
					break;
				}
			}

			for (int i = 0; i < parameters.Length; i++)
			{
				if (parameters[i]?.Name?
						.Equals("model", StringComparison.InvariantCultureIgnoreCase) ?? false
						&& parameters[i].ParameterType == typeof(Category))
				{
					modelPosition = i;
					break;
				}
			}

			if (!idPosition.HasValue || !modelPosition.HasValue) return next;
			

			return async (invocationContext) =>
			{
				Dictionary<string, string[]> validationErrors = [];
				List<string> validationMessages = [];

				// first i check the id parameter has value
				int? id = invocationContext.GetArgument<int?>(idPosition.Value);
				if (id is null or <= 0)
				{
					return Results.Problem(
										title: "Error in Updating Model",
										statusCode: StatusCodes.Status400BadRequest,
										detail: "Model id had been sent is null");
				}	


				// second i check model sended to the api body not null
				Category? model = invocationContext.GetArgument<Category?>(modelPosition.Value);
				if(model is null)
				{
					return Results.Problem(
										title: "Category Model is invalid",
										statusCode: StatusCodes.Status400BadRequest,
										detail: "Category Model is null");
				} 
				

				// third i validate the model properties
				if (string.IsNullOrWhiteSpace(model.Title))
					validationMessages.Add("Enter Category Title");

				if (model.Id == 0)
					validationMessages.Add("category id is invalud");

				if(validationMessages.Count > 0)
				{
					validationErrors.Add("ModelValidationErrors", [.. validationMessages]);
					return Results.ValidationProblem(
													validationErrors,
													title: "Category Model is invalid",
													statusCode: StatusCodes.Status400BadRequest,
													detail: "Correct Model Validation Errors");
				}

				return await next(invocationContext);
			};
		}
	}
}
