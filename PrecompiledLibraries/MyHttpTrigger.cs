using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

using Microsoft.Azure.WebJobs.Host;

namespace PrecompiledLibraries
{
    /// <summary>
    /// This represents the entity for HTTP trigger.
    /// </summary>
    public class MyHttpTrigger
    {
        /// <summary>
        /// Runs and processes the function method.
        /// </summary>
        /// <param name="req"><see cref="HttpRequestMessage"/> object.</param>
        /// <param name="log"><see cref="TraceWriter"/> instance.</param>
        /// <returns>Returns the <see cref="HttpResponseMessage"/> object.</returns>
        public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
        {
            log.Info($"C# HTTP trigger function processed a request. RequestUri={req.RequestUri}");

            // parse query parameter
            string name = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "name", true) == 0)
                .Value;

            // Get request body
            dynamic data = await req.Content.ReadAsAsync<object>().ConfigureAwait(false);

            // Set name to query string or body data
            name = name ?? data?.name;

            var res = new HttpResponseMessage();
            if (name == null)
            {
                res.StatusCode = HttpStatusCode.BadRequest;
                res.Content = new ObjectContent<string>("Please pass a name on the query string or in the request body", new JsonMediaTypeFormatter());
                return res;
            }

            res.StatusCode = HttpStatusCode.OK;
            res.Content = new ObjectContent<string>($"Hello {name}", new JsonMediaTypeFormatter());

            return res;
        }
    }
}
