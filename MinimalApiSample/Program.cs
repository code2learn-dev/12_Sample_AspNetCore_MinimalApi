using MinimalApiSample.MiddleWares;
using MinimalApiSample.ModelBinding;
using MinimalApiSample.Models;
using MinimalApiSample.RoutingMinimalApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<CategoryRepositoy>();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();


// to convert exceptions catched bt ExceptionHandlerMiddleware to Problem Details 
// we have to use IProblemDetailsService middleware
// for complete this way we must using ExceptionHandlerMiddleware
builder.Services.AddProblemDetails();

builder
	// config some configuration options
	.ConfigApplicationOptions()
	// config link generator Route Options
	.ConfigureLinkGenerationWithRouteOptions();


// config minimal api json 
builder.Services.ConfigJsonBodyInComplexTypes();


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
//app.FindCategoryWithFilterFactory()
//   .DeleteCategoryWithFilterFactory()
//   .EditCategoryWIthFilterFactory();


// استفاده کردن از اینترفیس IEndpointFilter
// کلاسی که از این اینترفیس ارث بری می کند یک متد بت نام InvokeAsync
// را پیاده سازی نماید که آرگومان های ورودی آن کاملا مشابه آرگومان
// بکار گرفته شده در یک فیلتر معمولی است
//app.FindCategoryEndpointFilter();


/*
  به مرور زمان با زیاد شدن اندپوینت ها که به ازای هر یک از موجودیت ها باید
  چهار عمل اصلی را برای هر یک پیاده سازی نماییم بنابراین در چنین حالتی
  کدهای تکراری به ازای هر یک از اندپویت ها افزایش می یابد و از طرفی در صورتی که
  ما فیلتری را برای هر یک از اندپوینت ها تعریف کرده باشیم احتمال فراموش کردن
  اضافه کردن فیلتر به یک اندپوینت وجود دارد بنابراین در این حالت جهت کاهش تکرار 
  و همچنین جهت اینکه بتوانیم به یک باره یک فیلتر تعریف شده را به تمامی اندپوینت ها
  با مسیر پایه یکسان اضافه کنیم از Route Group ها استفاده می کنیم
 */
//app.ConfigureCategoryMapGroups()
//	.CategoryMapGetRouteGroup()
//	.CategoryMapGetByIdRouteGroup()
//	.CategoryMapPostRouteGroup()
//	.CategoryMapDeleteRouteGroup()
//	.CategoryMapPutRouteConfig();


// routing samples in minimal api 
//app.FindCategoryByIdRouting();


// using link generator in minimal apis
//app
//	.GenerateCategoryListLinkGenerator()
//	.GetCategoryByIdLink();


// all over minimal apis model binding states
app
	//.SimpleTypeModelBinding()
	//.ArrayModelBindig()
	//.DefaultValueModelBinding()
	//.CustomBinding()
	.SearchCategoryByAsParameter()
	.AddCategoryWithParameterValidationFilter()
	.AddCategoryJustWithIdValidation();

app.Run();
