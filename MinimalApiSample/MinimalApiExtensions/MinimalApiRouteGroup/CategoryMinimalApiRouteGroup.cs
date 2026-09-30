using MinimalApiSample.Filters;
using MinimalApiSample.Models;

namespace MinimalApiSample.MinimalApiExtensions.MinimalApiRouteGroup
{
    public static class CategoryMinimalApiRouteGroup
    {
		static RouteGroupBuilder _categoryMapGroup;
		static RouteGroupBuilder mapGroupWithIdFilter;
		static RouteGroupBuilder mapGroupWithModelFilter;
		static RouteGroupBuilder mapGroupWithFilterFactory;


		/*
			در صورت استفاده مشترک از پیشوند تعریف شده برای رویت گروپ ها در این صورت
			حتی اگر شما یک فیلتر را به هر یک از روت گروپ های بعدی تعریف شده اعمال کنید
			مطمئن باشید در این صورت این فیلتر برای تمامی روت گروپ هایی که دارای پیشوند مشترک 
             هستند استفاده می شود بنابراین بهتر است همواره مسیرهایی که از فیلترهای متفاوت استفاده
			می کنند را از سایر روت گروپ ها جدا نمایید
		 */
		/*		public static WebApplication ConfigureCategoryMapGroups(this WebApplication app)
				{
					_categoryMapGroup = app.MapGroup("/Category");

					mapGroupWithIdFilter =
						_categoryMapGroup.AddEndpointFilter<CategoryIdValidationFilter>();

					mapGroupWithModelFilter =
						_categoryMapGroup.AddEndpointFilter(SimpleValidationFilters.ValidateCategoryModel);

					mapGroupWithFilterFactory =
						_categoryMapGroup.AddEndpointFilterFactory(CategoryFilterFactory.ValidateCategoryModel);

					return app;
				}*/

		// روش درست برای تعریف کردن روت گروپ ها جدا کردن آنها از یکدیگر است
		public static WebApplication ConfigureCategoryMapGroups(this WebApplication app)
		{
			_categoryMapGroup = app.MapGroup("/Category"); 
			
			mapGroupWithIdFilter = app.MapGroup("/Category")
					.AddEndpointFilter<CategoryIdValidationFilter>();

			mapGroupWithModelFilter = app.MapGroup("/Category")
					.AddEndpointFilter(SimpleValidationFilters.ValidateCategoryModel);

			mapGroupWithFilterFactory = app.MapGroup("/Category")
					.AddEndpointFilterFactory(CategoryFilterFactory.ValidateCategoryModel);

			return app;
		}

		public static WebApplication CategoryMapGetRouteGroup(this WebApplication app)
        { 
			_categoryMapGroup.MapGet("/", (CategoryRepositoy repository) =>
			{
				IReadOnlyCollection<Category> categories = repository.Categories;
				return TypedResults.Ok(categories);
			});

			return app;
        }

        public static WebApplication CategoryMapGetByIdRouteGroup(this WebApplication app)
        {
			mapGroupWithIdFilter.MapGet("/{id:int}", (int id, CategoryRepositoy repository) =>
			{
				Category? category = repository.Find(id);
				return category is not null
					? TypedResults.Ok(category)
					: Results.Problem(
									title: "Category Not Found",
									statusCode: StatusCodes.Status404NotFound,
									detail: $"Category with id: {id} not found");
			});

			return app;
        }

        public static WebApplication CategoryMapPostRouteGroup(this WebApplication app)
        {
			mapGroupWithModelFilter.MapPost("/", (Category model, CategoryRepositoy repository) =>
			{
				Category? category = repository.Add(model);
				return category is not null
					? TypedResults.Ok(category)
					: Results.Problem(
									title: "Error in Create Category ",
									statusCode: StatusCodes.Status400BadRequest,
									detail: "for more information see the log file");
			});

			return app;
        }


		public static WebApplication CategoryMapPutRouteConfig(this WebApplication app)
		{
			mapGroupWithFilterFactory.MapPut("/{id:int?}", (int? id, Category model, CategoryRepositoy repository) =>
			{
				Category? category = repository.Update(model);
				return category is not null
				   ? TypedResults.Ok(category)
				   : Results.Problem(
								   title: "Error in Update Category ",
								   statusCode: StatusCodes.Status400BadRequest,
								   detail: "for more information see the log file");
			});

			return app;
		}

		public static WebApplication CategoryMapDeleteRouteGroup(this WebApplication app)
		{
			mapGroupWithIdFilter.MapDelete("/{id:int}", (int id, CategoryRepositoy repository) =>
			{
                Category? category = repository.Delete(id);
				return category is not null
				   ? TypedResults.Ok(category)
				   : Results.Problem(
								   title: "Error in Delete Category ",
								   statusCode: StatusCodes.Status400BadRequest,
								   detail: "for more information see the log file");
			});

			return app;
		}
    }
}
