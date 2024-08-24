using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp.Services
{
    public class Interceptor : DelegatingHandler
    {

        public Interceptor(HttpMessageHandler innerHandler) : base(innerHandler)
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(Configuration.Configuration.TOKKEN))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Configuration.Configuration.TOKKEN);
            }

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                if (Application.OpenForms["frmMain"] != null)
                {
                    var mainForm = Application.OpenForms["frmMain"];
                    mainForm.Invoke(new MethodInvoker(delegate
                    {
                        using (var loginForm = new frmLogin())
                        {
                            var result = loginForm.ShowDialog();
                            if (result == DialogResult.OK)
                            {
                                Configuration.Configuration.TOKKEN = Configuration.Configuration.TOKKEN;
                                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Configuration.Configuration.TOKKEN);

                                response = base.SendAsync(request, cancellationToken).Result;
                            }
                        }
                    }));
                }
            }

            return response;
        }
    }
}
