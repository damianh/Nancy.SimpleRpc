namespace Nancy.SimpleRpc.Client
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    public class ServiceClient : IServiceClient
    {
        private readonly string _baseUri;
        private readonly HttpMessageHandler _httpMessageHandler;

        public ServiceClient(string baseUri)
        {
            _baseUri = baseUri;
        }

        public ServiceClient(string baseUri, HttpMessageHandler httpMessageHandler)
        {
            _baseUri = baseUri;
            _httpMessageHandler = httpMessageHandler;
        }

        public async Task<TResponse> Send<TResponse>(object request, CancellationToken cancellationToken)
            where TResponse : new()
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            HttpClient httpClient = (_httpMessageHandler != null) ? new HttpClient(_httpMessageHandler) : new HttpClient();
            string requestJson = JsonConvert.SerializeObject(request);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response =
                await httpClient.PostAsync(_baseUri + "/" + request.GetType().Name,
                        new StringContent(requestJson, Encoding.UTF8, "application/json"),
                        cancellationToken);
            string responseString = await response.Content.ReadAsStringAsync();
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new InvalidOperationException(string.Format("Oops, was expecting an OK but got a {0}. Here's the body:{1}{1}{2}",
                    response.StatusCode,
                    Environment.NewLine,
                    responseString));
            }
            return JsonConvert.DeserializeObject<TResponse>(responseString);
        }
    }
}