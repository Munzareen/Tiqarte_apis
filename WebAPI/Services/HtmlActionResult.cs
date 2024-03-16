using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

public class HtmlActionResult : IHttpActionResult
{
    private readonly string htmlContent;
    private readonly HttpStatusCode statusCode;

    public HtmlActionResult(string htmlContent, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        this.htmlContent = htmlContent;
        this.statusCode = statusCode;
    }

    public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
    {
        var response = new HttpResponseMessage(statusCode);
        response.Content = new StringContent(htmlContent, Encoding.UTF8, "text/html");
        return Task.FromResult(response);
    }
}
