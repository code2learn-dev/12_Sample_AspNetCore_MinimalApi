
namespace MinimalApiSample.Filters
{
    public class CategoryIdValidationFilter : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(
            EndpointFilterInvocationContext context, 
            EndpointFilterDelegate next)
        {
            int id = context.GetArgument<int>(0);
            if(id <= 0)
            {
                return Results.Problem(
                                        title: "id is invalid",
                                        statusCode: StatusCodes.Status400BadRequest,
                                        detail: "category id is less than or equal 0");
            }

            return await next(context);
        }
    }
}
