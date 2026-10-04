using Microsoft.AspNetCore.Mvc;
using MinimalApiSample.Models;
using System.Diagnostics.Eventing.Reader;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace MinimalApiSample.ModelBinding
{
	public static class CategoryMinimalApiModelBinding
	{

		/// <summary>
		/// کانفیگ کردن تنظیمات مربئط به سریالایز کردن داده ها
		/// به همراه کانقیگ کردن قوانین نوشتاری مربوط به مقادیر اشیای json
		/// </summary>
		/// <param name="services"></param>
		public static void ConfigJsonBodyInComplexTypes(this IServiceCollection services)
		{
			services.ConfigureHttpJsonOptions(cfg =>
			{
				cfg.SerializerOptions.AllowTrailingCommas = true;
				cfg.SerializerOptions.PropertyNameCaseInsensitive = false;
				cfg.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
			});
		}


		public static WebApplication SimpleTypeModelBinding(this WebApplication app)
		{
			// در این حالت پارامتر همنام با پارامتر مسیریابی هم نام باشد
			//app.MapGet("/Category/{id}", (int id, CategoryRepositoy repository) =>
			//{
			//    var category = repository.Find(id);
			//    return category is not null
			//        ? TypedResults.Ok(category)
			//        : Results.Problem(
			//                            title: "Category Not Found",
			//                            statusCode: StatusCodes.Status404NotFound,
			//                            detail: $"Category with id: {id} not found");
			//});

			// در این حالت مقدار آیدی باید از قسمت کوئری استرینگ مقداردهی شود در غیر
			// اینصورت با خطای زمان احرا مواجه می شود
			app.MapGet("/Category", (int id, CategoryRepositoy repository) => { });

			// در این حالت حتی در صورت مشخص نکردن مقداری برای آیدی به شما
			// خطا نمی دهد فقط مقدار نال برای آیدی مشخص می شود
			//app.MapGet("/Category/{id?}", (int? id) => { });

			app.MapGet("/Category/{id}",
				(int id,
				[FromHeader(Name = "pageSize")] string size,
				[FromQuery] string sort) =>
				{ });
			return app;
		}


		/// <summary>
		/// گرفتن لیستی از پارامترهای هم نام در یک متغیر
		/// </summary>
		/// <param name="app"></param>
		/// <returns
		/// ></returns>
		public static WebApplication ArrayModelBindig(this WebApplication app)
		{
			//app.MapGet("/Category/search", (int[] id) =>
			//{
			//    return string.Join(", ", id);
			//});


			// identify different name for endpoint argument
			app.MapGet("/Category/search", ([FromQuery(Name = "id")] int[] idList) =>
			{
				return string.Join(", ", idList);
			});

			return app;
		}

		/// <summary>
		/// Default value in model binding
		/// </summary>
		/// <param name="app"></param>
		/// <returns></returns>
		public static WebApplication DefaultValueModelBinding(this WebApplication app)
		{
			// before .Net 11 we couldn't set default value for lambda function parameter so we using the local function instead
			app.MapGet("/Caegory/{id}", CategoryIdWithDefaultValue);
			string CategoryIdWithDefaultValue(int id = 1)
				=> $"category id is : {id}";

			// after .Net 12
			app.MapGet("/Category", (int id = 1) => { });

			return app;
		}

		/// <summary>
		/// Using custom model binder must create method named BindAsync 
		/// </summary>
		/// <param name="app"></param>
		/// <returns></returns>
		public static WebApplication CustomBinding(this WebApplication app)
		{
			app.MapPost("/Category", (CategoryDetail? model) =>
			{
				if (model is not null)
					return $"Category id: {model?.id} and title: {model?.title}";
				else
					return $"model is invalid";
			});

			return app;
		}

		/// <summary>
		/// زمانی که تعداد پارامترهای اندپوینت زیاد باشد برای اینکه بتوانیم خوانایی
		/// کد نوشته شده برای اندپوینت را زیاد کنیم از خصوصیت AsParameter استفاده می
		/// کنیم که تمامی آرگومان ها را در قالب یک کلاس یا استراکت در بر می گیرد و
		/// خوانایی آن را نیز بیشتر خواهد کرد
		/// </summary>
		/// <param name="app"></param>
		/// <returns></returns>
		public static WebApplication SearchCategoryByAsParameter(this WebApplication app)
		{
			app.MapGet("/Category/Search", ([AsParameters] SearchCategoryModel model) => { });

			return app;
		}


		/// <summary>
		/// زمانی که بخواهیم از اعتبارسنجی در مینیمال ای پی آی های خود استفاده کنیم
		/// می توانیم از فیلترها استفاده کنیم که برای این کار ایده عال هستند ولی در 
		/// این حالت ما باید خودمان اعتبارسنجی را پیاده سازی کنیم از طرفی انعطاف پذیری
		/// بیشتری را می توانیم در اعتبارسنحی پیاده سازی کنیم ولی از طرفی ما بجای 
		/// استفاده از ای پی آی های از پیش ساخته باید خودمان آن را بنویسیم
		/// ولی ما می توانیم از یک پکیج برای این کار استفاده کنیم ابتدا باید پکیج زیر
		/// را نصب کنیم :
		/// MinimalApis.Extensions
		/// که این پکیج یک متد توسعه یافته را در اختیار شما قرار می دهد به نام 
		/// WithParameterValidation()
		/// که یک سیستم اعتبارسنجی ساده است که بر روی DataAnnotation سوار می شود و 
		/// ما می توانیم این متد را به انتهای اندپوینت خود اضافه کنیم
		/// </summary>
		/// <param name="app"></param>
		/// <returns></returns>
		public static WebApplication AddCategoryWithParameterValidationFilter(
			this WebApplication app)
		{
			app.MapPost("/Category/Add", 
				(Category model, CategoryRepositoy repository) =>
			{
                Category? category = repository.Add(model);
				return category is not null
					? TypedResults.Ok(category)
					: Results.Problem(
										title: "Error in Add Category",
										statusCode: StatusCodes.Status400BadRequest,
										detail: "for more information correct the validation errors");
			}).WithParameterValidation();

			return app;
		}


		/// <summary>
		/// همواره امکان اعتبارسنجی پارامترهای مسیریابی که از نوع داده های
		///  پایه هستند وجود ندارد بنابراین برای این کار می توانیم از خصوصیت
		/// AsParameters
		/// استفاده کنیم و اعتبارسنجی خود را برای پارامترهای ساده اعمال کنیم
		/// </summary>
		/// <param name="app"></param>
		/// <returns></returns>
		public static WebApplication AddCategoryJustWithIdValidation(
			this WebApplication app)
		{
			app.MapPost("/Caegory/{id?}", 
				([AsParameters] CategoryIdValidation model) => {
					return model.Id;
				}).WithParameterValidation();
			return app;
		}
	}
}
