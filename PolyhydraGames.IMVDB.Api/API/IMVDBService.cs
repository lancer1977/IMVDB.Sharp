using System.Diagnostics;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using PolyhydraGames.Core.Interfaces;
using PolyhydraGames.IMVDB.DTO;
using PolyhydraGames.IMVDB.Responses;

namespace PolyhydraGames.IMVDB.API
{
    public class IMVDBService
    {
        public static JsonSerializerOptions DefaultOptions = new()
        {
            Converters = { new NullableIntEmptyStringConverter(), new ImageConverter(), new IntStringConverter() }
        };

        private readonly IIMVDBAuthorization _authService;
        protected readonly IHttpService _httpService;
        private readonly ILogger<IMVDBService> Logger;

        public IMVDBService(ILogger<IMVDBService> logger, IIMVDBAuthorization authService, IHttpService httpService)
        {
            Logger = logger;
            _authService = authService;
            _httpService = httpService;
            Options = DefaultOptions;
        }

        public static string BaseUrl => "https://imvdb.com/api/v1/";

        public JsonSerializerOptions Options { get; set; }

        public static T? Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, DefaultOptions);
        }

        protected async Task<HttpRequestMessage> GetHttpRequestMessage(
            string method,
            HttpMethod httpMethod,
            HttpContent content = null)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = Uri(method),
                Method = httpMethod,
                Content = content
            };
            Debug.WriteLine(request.RequestUri);
            request.Headers.Add("IMVDB-APP-KEY", _authService.APIKey);
            return request;
        }

        private Uri Uri(string method)
        {
            var endpoint = BaseUrl + method;
            var uri = new Uri(endpoint);
            return uri;
        }

        protected async Task<HttpResponseType<T>> Get<T>([CallerMemberName] string endUrl = "")
        {
            var client = _httpService.GetClient;
            try
            {
                var request = await GetHttpRequestMessage(endUrl, HttpMethod.Get);
                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var rawText = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine(rawText);
                    var obj = Deserialize<T?>(rawText);
                    return HttpResponse.Create("", obj);
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return HttpResponse.Create("Not authorized", default(T));
                }

                return HttpResponse.Create(response.StatusCode + ":" + response.ReasonPhrase, default(T));
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, "Error in Get");
                return HttpResponse.Create(ex.Message, default(T));
            }
        }

        public Task<HttpResponseType<VideoResponse>> GetVideoResponse(string id)
        {
            var endUrl = $"video/{id}?include=credits,bts,countries";
            return Get<VideoResponse>(endUrl);
        }

        public async Task<HttpResponseType<SearchResult<Video>>> SearchVideos(string value, int per_page = 25,
            int page = 1)
        {
            var endUrl = $"search/videos?q={value.Replace(" ", "+")}&per_page={per_page}&page={page} ";
            var result = await Get<SearchResult<Video>>(endUrl);
            return result;
        }

        public async Task<HttpResponseType<SearchResult<EntityResponse>>> SearchEntities(string value,
            int per_page = 25, int page = 1)
        {
            var endUrl = $"search/entities?q={value.Replace(" ", "+")}&per_page={per_page}&page={page} ";
            var result = await Get<SearchResult<EntityResponse>>(endUrl);
            return result;
        }

        public async Task<HttpResponseType<EntityResponse>> GetEntity(string value)
        {
            var endUrl = $"entity/{value}?include=credits,distinctpos";
            var result = await Get<EntityResponse>(endUrl);
            return result;
        }
    }

    public record HttpResponseType<T>(bool Success, string Message, T? Value);

    public static class HttpResponse
    {
        public static HttpResponseType<T> Create<T>(string message, T? value)
        {
            return new HttpResponseType<T>(value != null, message, value);
        }
    }
}