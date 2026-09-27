using MinimalApiSample.Models;

namespace MinimalApiSample.Filters
{
    public static class SimpleValidationFilters
    {
        public static async ValueTask<object?> ValidateCategoryId(
            EndpointFilterInvocationContext context,
            EndpointFilterDelegate next)
        {
            int? id = context.GetArgument<int>(0);
            if(id is null or <= 0)
            {
                return Results.ValidationProblem(
                    new Dictionary<string, string[]>
                    {
                        { "ValidationErrors", ["Please Enter Category Id"] }
                    });
            }

            return await next(context);
        }

        public static async ValueTask<object?> ValidateCategoryModel(
            EndpointFilterInvocationContext context,
            EndpointFilterDelegate next)
        {
            Category? model = context.GetArgument<Category>(0);
            if(model is null)
            {
                return Results.ValidationProblem(
                    new Dictionary<string, string[]>
                    {
                        { "ValidationErrors", ["Model is null"] }
                    });
            }

            Dictionary<string, string[]> validationErrors = [];
            List<string> validationMessages = [];
            
            if(model.Id <= 0)
                validationMessages.Add("Category Id is invalid");
            if (string.IsNullOrEmpty(model.Title) || string.IsNullOrWhiteSpace(model.Title))
                validationMessages.Add("Enter Category Title");

            if(validationMessages.Count > 0)
            {
                validationErrors.Add("ValidationErrors", validationMessages.ToArray());
                return Results.ValidationProblem(validationErrors);
            }

            return await next(context);
        }
    }
}
