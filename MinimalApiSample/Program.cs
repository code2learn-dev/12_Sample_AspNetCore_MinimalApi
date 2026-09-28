using MinimalApiSample.MiddleWares;
using MinimalApiSample.MinimalApiExtensions;
using MinimalApiSample.MinimalApiExtensions.MinimalApiWithFilter;
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
//app
//	.AllCategoriesProblemResult()
//	.FindByIdProblemResult()
//	.AddProblemResult()
//	.EditProblemResult()
//	.DeleteProblemResult();


// adding category minimal apis with endpoint filters
//app
//	.FindCategoryValidateFilter()
//	.AddCategoryValidationFilter();

/*
 در زمان استفاده کردن از فیلترهای معمولی هر یک از آرگومان های درخواستی باید
 دقیقا در محل خواسته شده قرار داشته باشد به عنوان مثال در زمان تعریف کردن
 یک مینیمال ای پی آی که دارای 2 آرگومان است امکان قرار گرفتن آرگومان آیدی
 در آرگومان اول و یا دوم وجود دارد بنابراین امکان دسترسی به آرگومانی با ترتیب
 اشتباه در زمان استفاده کردن از فیلترهای معمولی وجود دارد بنابرایت ما می توانیم
 در چنین مواقعی از فیلتر فکتوری های استفاده کنیم که امکان تعریف یک فیلتر با
 قابلیت استفاده بر روی چندین ایندپوینت را فراهم می کند
 فیلتر فکتوری یک فیلتر فاکنشن را به عنوان خروحی باز می گرداند که این فیلتر
 بازگشت داده شده با پایپ لاین اصلی ترکیب می شود
 */
app.FindCategoryWithFilterFactory()
   .DeleteCategoryWithFilterFactory()
   .EditCategoryWIthFilterFactory();

app.Run();
