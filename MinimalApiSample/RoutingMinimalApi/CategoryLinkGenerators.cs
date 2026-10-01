using Microsoft.Extensions.Options;
using MinimalApiSample.Models;

namespace MinimalApiSample.RoutingMinimalApi
{
    public static class CategoryLinkGenerators
    {
        /*
         ما در اپلیکیشن های خود همواره در برخی صفحات از یکسری لینک های ثابت استفاده می کنیم 
         که با استفاده از آن لینک ها کاربران را به یک مسیر خاص هدایت می کنیم 
         ولی در صورتی که تصمیم بگیریم تا مسیری که کاربر به آنها هدایت می شود را تغییر دهیم
         در این صورت باید لینک های تمامی صفحات را تغییر دهیم
         راه حل این است که ما برای آن مسیر مشخص که قرار است کاربر به آن هدایت شود یک نام اختصاص 
         می دهیم و سپس در تمامی لینک های ثابت در تمامی صفحات فقط از آن نام مستعاری که برای
         آدرس صفحه مشخص کرده ایم استفاده می کنیم در این صورت هر زمان که بخواهیم آن لینک ثابت را
         تغییر دهیم مشکلی برای لینک های ثابت پیش نمی آید چون آن مستعار نمایند آدرس تعریف شده به
         مسیر مقصد است اما برای استفاده از این نام مستعار ما از قابلیتی به نام لینک جنریتور
         استفاده می کنیم که در این حالت لینک جنریتور یک اسم که به مسیر مقصد اشاره دارد گرفته
         و در خروجی یک لینکی را که به مسیر مقصد اشاره دارد را تولید می نماید


         در حالت دیگر می تواند لینک جنریتور را به این صورت توصیف کرد که در حالت عادی مینیمال ای پی آی
        به این صورت عمل می کند که ابتدا نسبت به یک یوآرآل ورودی یک الگوی مسیریابی را پیدا کرده و 
        درخواست ورودی را به آن تطبیق می دهد یعنی مپ می کند و سپس نسبت به پارامترهای مورد درخواست در 
        الگوی مسیریابی تعریف شده در اندپوینت مقادیر قید شده در آدرس یوآرآل را به آن پارامترها نسبت می 
        دهد یعنی ما از یک آدرس یوآرال به یک اندپوینت می رسیم ولی در حالت لینک جنریتور کاملا برعکس
        حالت قبلی است و ما از یک اندپوینت اولیه به یک آدرس یوآرال می رسیم یعنی لینک جنریتور یک 
        اندپوینت را به یک آدرس یوآرال تبدیل می کند
         */ 

		public static WebApplication GenerateCategoryListLinkGenerator(this WebApplication app)
        {
            app.MapGet("/Category", (CategoryRepositoy repository) =>
            {
                IReadOnlyCollection<Category> categories = repository.Categories;
                return TypedResults.Ok(categories);
            }).WithName("categories");

            app.MapGet("/AllCategoriesLink", (
                LinkGenerator link,
                IOptions<AppOptions> options) =>
            {
                var categoriesLink = link.GetPathByName("categories");
                return $"To see all categories list visit: {options.Value.BaseHostUrl}{categoriesLink}";
            });

            return app;
        }


        public static WebApplication GetCategoryByIdLink(this WebApplication app)
        {
            app.MapGet("/Category/{id}", (int id, CategoryRepositoy repository) =>
            {
                Category? category = repository.Find(id);
                return category is not null
                    ? TypedResults.Ok(category)
                    : Results.Problem(
                                       title: "Category Not Found",
                                       statusCode: StatusCodes.Status404NotFound,
                                       detail: $"Category not found with id: {id}");
            }).WithName("find_category");

            app.MapGet("/FirstCategoryLink", (
                LinkGenerator link,
                IOptions<AppOptions> options) =>
            {
                string? linkGeneratord = link.GetPathByName("find_category", new { id = 1 });
                return $"To see the first category visit this link: {options.Value.BaseHostUrl}{linkGeneratord}";
            });
          

            return app;
        }

        public static WebApplicationBuilder ConfigApplicationOptions(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<AppOptions>(
                builder.Configuration.GetSection(AppOptions.SectionName));

            return builder;
        }


        public static void ConfigureLinkGenerationWithRouteOptions(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
                options.AppendTrailingSlash = true;
                options.LowercaseQueryStrings = true;
            });
        }
    }
}
