namespace Nancy.SimpleRpc.Client
{
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

        public async Task<TResponse> Send<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
            where TResponse : new()
        {
            HttpClient httpClient = (_httpMessageHandler != null) ? new HttpClient(_httpMessageHandler) : new HttpClient();
            string requestJson = JsonConvert.SerializeObject(request);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = await httpClient.PostAsync(
                _baseUri + "/" + typeof(TRequest).Name,
                new StringContent(requestJson, Encoding.UTF8, "application/json"),
                cancellationToken);
            string responseJson = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TResponse>(responseJson);
        }
    }
}