using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace WinFormsApp.Services
{
    public class Interceptor : DelegatingHandler
    {
        private string _token;

        public Interceptor(HttpMessageHandler innerHandler) : base(innerHandler)
        {
            _token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6ImFzbWl0aEBleGFtcGxlLmNvbSIsImlkIjo1MSwiZmlyc3RfbmFtZSI6IkFsaWNlIiwibGFzdF9uYW1lIjoiQWxpY2UiLCJ1c2VybmFtZSI6ImFzbWl0aCIsInByb2ZpbGVfcGljdHVyZSI6Imh0dHBzOi8vcmFuZG9tdXNlci5tZS9hcGkvcG9ydHJhaXRzL3dvbWVuLzEuanBnIiwidHlwZSI6IkFETUlOIiwianRpIjoiNTEtMTcyNDQ0MDcwNTE4NSIsImlhdCI6MTcyNDQ0MDcwNSwiZXhwIjoxNzI0NjEzNTA1fQ.8DvqMhMyRM3Ri9eCuaF8sNoyuVjrXY39PwT66nX6yPw";
        }

        public void SetToken(string token)
        {
            _token = token;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(_token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
