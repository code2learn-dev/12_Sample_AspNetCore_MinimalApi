using MinimalApiSample.MiddleWares;
using MinimalApiSample.MinimalApiExtensions;
using MinimalApiSample.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<CategoryRepositoy>();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();


// to convert exceptions catched bt ExceptionHandlerMiddleware to Problem Details 
// we have to use IProblemDetailsService middleware
// for complete this way we must using ExceptionHandlerMiddleware
builder.Services.AddProblemDetails();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
	app.UseSwagger();
	app.UseSwaggerUI(options =>
	{
		options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
		options.RoutePrefix = string.Empty;
	});
}
else
{
	// for convert exceptions to the problem details
	// we must use ExceptionHandlerMiddleware without 
	// providing an error handling path
	app.UseExceptionHandler();
}

	app.UseMiddleware<NotFoundMiddleware>();

// simple minimal api
// sending direct data to the output as response
//app
// create first minimal api
//.MapSimpleMinimalApi()
// map category minimal apis
//.GetCategoriesApi()
//.AddCategory()
//.FindCategory()
//.EditCategory();

// add minimal apis with specified result
// using TypedResult and Result to set minimal api response status
//app
//	.GetCategiriesResult()
//	.AddCategoryResult()
//	.UpdateCategoryResult()
//	.FindCategoryResult();


// using Problem Details and ValidationProblem Details
// for generating consistent response for all minimal api's
app
	.AllCategoriesProblemResult()
	.FindByIdProblemResult()
	.AddProblemResult()
	.EditProblemResult()
	.DeleteProblemResult();

app.Run();
