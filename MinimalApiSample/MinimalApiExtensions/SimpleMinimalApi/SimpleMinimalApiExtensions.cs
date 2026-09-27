using MinimalApiSample.Models;
using System.Net.Mime;

namespace MinimalApiSample.MinimalApiExtensions.SimpleMinimalApi
{
	public static class SimpleMinimalApiExtensions
	{
		public static WebApplication MapSimpleMinimalApi(this WebApplication app)
		{
			app.MapGet("/", async context =>
            {
				context.Response.ContentType = MediaTypeNames.Text.Plain;
				await context.Response.WriteAsync("Home Page");
			});
			return app;
		} 
	}
}
