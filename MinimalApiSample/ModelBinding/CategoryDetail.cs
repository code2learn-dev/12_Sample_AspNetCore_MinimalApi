namespace MinimalApiSample.ModelBinding
{
    /// <summary>
    /// For creating custom binding to encapsulate the logic for
    /// extracting the data you need we can either using HttpContext
    /// object and excapsulate login by adding the following method to
    /// the following class defined as custom model binding 
    /// 
    /// in this class body contain raw text and we read body
    /// with StreamReader and we enter simple text in two line like below :
    /// 1
    /// title
    /// </summary>
    /// <param name="id"></param>
    /// <param name="title"></param>
    public record CategoryDetail(int id, string title)
    {
        public static async ValueTask<CategoryDetail?> BindAsync(HttpContext context)
        {
            using var sr = new StreamReader(context.Request.Body);

            string? line1 = await sr.ReadLineAsync(context.RequestAborted);
            if (line1 is null) return null;

            string? line2 = await sr.ReadLineAsync(context.RequestAborted);
            if (line2 is null) return null;

            if (int.TryParse(line1, out int n1))
            {
                return (CategoryDetail?)new CategoryDetail(n1, line2);
            }
            else
            {
                return null;
            }
        }
    }
}
