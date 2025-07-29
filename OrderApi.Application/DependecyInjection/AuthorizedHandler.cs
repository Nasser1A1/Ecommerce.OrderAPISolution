using Microsoft.AspNetCore.Http;

namespace OrderApi.Application.DependecyInjection
{
    public class AuthorizedHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthorizedHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var accessToken = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();

            if (!string.IsNullOrEmpty(accessToken))
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken.Replace("Bearer ", ""));
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}