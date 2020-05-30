using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

public class AuthorizationHeaderHandler : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        string appKey = ConfigurationManager.AppSettings["AppKey"];

        IEnumerable<string> apiKeyHeaderValues = null;
        if (request.Headers.TryGetValues("x-api-key", out apiKeyHeaderValues))
        {
            var apiKeyHeaderValue = apiKeyHeaderValues.First();
            if(apiKeyHeaderValue == appKey)
            {
                var usernameClaim = new Claim(ClaimTypes.Name, "AcceptedUser");
                var identity = new ClaimsIdentity(new[] { usernameClaim }, "ApiKey");
                var principal = new ClaimsPrincipal(identity);
                Thread.CurrentPrincipal = principal;
            }
            else
            {
                return Task.FromResult(Execute(request));
            }
        }
        else
        {
            return Task.FromResult(Execute(request));
        }
        return base.SendAsync(request, cancellationToken);
    }
    private HttpResponseMessage Execute(HttpRequestMessage requestMessage)
    {
        HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
        response.RequestMessage = requestMessage;
        response.ReasonPhrase = "Unauthorized request";
        return response;
    }
}