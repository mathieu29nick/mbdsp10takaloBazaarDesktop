using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WinFormsApp.Services
{
    public class TokenHandler : DelegatingHandler
    {
        private readonly SessionService _sessionService;

        public TokenHandler(SessionService sessionService)
        {
            _sessionService = sessionService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = _sessionService.GetToken();
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Add("Authorization", $"Bearer {token}");
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
