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
            HttpContent? content = null)
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

                return HttpResponse.CreateError<T>(
                    MapErrorKind(response.StatusCode),
                    response.StatusCode,
                    BuildHttpErrorMessage(response),
                    endUrl);
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, "Error in Get");
                return HttpResponse.CreateError<T>(
                    IMVDBErrorKind.Exception,
                    null,
                    ex.Message,
                    endUrl);
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
            var endUrl = BuildSearchEndpoint("videos", value, per_page, page);
            var result = await Get<SearchResult<Video>>(endUrl);
            return result;
        }

        public async Task<HttpResponseType<SearchResult<EntityResponse>>> SearchEntities(string value,
            int per_page = 25, int page = 1)
        {
            var endUrl = BuildSearchEndpoint("entities", value, per_page, page);
            var result = await Get<SearchResult<EntityResponse>>(endUrl);
            return result;
        }

        public async Task<HttpResponseType<EntityResponse>> GetEntity(string value)
        {
            var endUrl = $"entity/{value}?include=credits,distinctpos";
            var result = await Get<EntityResponse>(endUrl);
            return result;
        }

        private static string BuildSearchEndpoint(string endpointType, string value, int perPage, int page)
        {
            var encoded = System.Uri.EscapeDataString(value).Replace("%20", "+");
            return $"search/{endpointType}?q={encoded}&per_page={perPage}&page={page}".Trim();
        }

        private static IMVDBErrorKind MapErrorKind(HttpStatusCode statusCode)
            => statusCode switch
            {
                HttpStatusCode.Unauthorized => IMVDBErrorKind.Unauthorized,
                HttpStatusCode.NotFound => IMVDBErrorKind.NotFound,
                HttpStatusCode.TooManyRequests => IMVDBErrorKind.RateLimited,
                HttpStatusCode.ServiceUnavailable => IMVDBErrorKind.TransientFailure,
                HttpStatusCode.BadGateway => IMVDBErrorKind.TransientFailure,
                HttpStatusCode.GatewayTimeout => IMVDBErrorKind.TransientFailure,
                _ when (int)statusCode >= 500 => IMVDBErrorKind.ServerError,
                _ => IMVDBErrorKind.HttpError
            };

        private static string BuildHttpErrorMessage(HttpResponseMessage response)
            => response.StatusCode switch
            {
                HttpStatusCode.Unauthorized => "Not authorized",
                HttpStatusCode.NotFound => "Not found",
                HttpStatusCode.TooManyRequests => "Rate limited",
                HttpStatusCode.ServiceUnavailable => "Service unavailable",
                HttpStatusCode.BadGateway => "Bad gateway",
                HttpStatusCode.GatewayTimeout => "Gateway timeout",
                _ => response.StatusCode + ":" + response.ReasonPhrase
            };
    }

    public enum IMVDBErrorKind
    {
        None = 0,
        Unauthorized = 1,
        HttpError = 2,
        Exception = 3,
        NotFound = 4,
        RateLimited = 5,
        TransientFailure = 6,
        ServerError = 7
    }

    public record IMVDBError(IMVDBErrorKind Kind, HttpStatusCode? StatusCode, string Message, string? Endpoint = null);

    public record HttpResponseType<T>(bool Success, string Message, T? Value, IMVDBError? Error);

    public static class HttpResponse
    {
        public static HttpResponseType<T> Create<T>(string message, T? value)
        {
            return new HttpResponseType<T>(value != null, message, value, null);
        }

        public static HttpResponseType<T> CreateError<T>(
            IMVDBErrorKind kind,
            HttpStatusCode? statusCode,
            string message,
            string? endpoint = null)
        {
            var error = new IMVDBError(kind, statusCode, message, endpoint);
            return new HttpResponseType<T>(false, message, default, error);
        }
    }
}
