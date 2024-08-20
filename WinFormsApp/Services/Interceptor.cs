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
            _token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6ImFzbWl0aEBleGFtcGxlLmNvbSIsImlkIjo1MSwiZmlyc3RfbmFtZSI6IkFsaWNlIiwibGFzdF9uYW1lIjoiQWxpY2UiLCJ1c2VybmFtZSI6ImFzbWl0aCIsInByb2ZpbGVfcGljdHVyZSI6Imh0dHBzOi8vcmFuZG9tdXNlci5tZS9hcGkvcG9ydHJhaXRzL3dvbWVuLzEuanBnIiwidHlwZSI6IkFETUlOIiwianRpIjoiNTEtMTcyNDEwMjg3MDk3NyIsImlhdCI6MTcyNDEwMjg3MCwiZXhwIjoxNzI0Mjc1NjcwfQ._0bw9bXIFF6twJtoecXFNFf4LNFbc0QkEvoPnau5-UM";
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
