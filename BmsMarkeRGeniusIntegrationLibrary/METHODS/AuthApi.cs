using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public static class AuthApi
{
    private static HttpClient _http;
    private static string _baseUrl = "http://192.168.3.10:9996/";

    static AuthApi()
    {
        InitializeHttpClient();
    }

    private static void InitializeHttpClient()
    {
        // DEV: TLS/Proxy/CRL beklemelerini kır
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        ServicePointManager.Expect100Continue = false;
        ServicePointManager.CheckCertificateRevocationList = false;
        ServicePointManager.DefaultConnectionLimit = 100;

        var handler = new HttpClientHandler
        {
            UseProxy = false,
            CheckCertificateRevocationList = false,
            ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        _http = new HttpClient(handler)
        {
            BaseAddress = new Uri(_baseUrl),
            Timeout = Timeout.InfiniteTimeSpan
        };
        _http.DefaultRequestHeaders.Accept.Clear();
        _http.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public static void SetBaseUrl(string baseUrl)
    {
        if (!string.IsNullOrEmpty(baseUrl) && baseUrl != _baseUrl)
        {
            _baseUrl = baseUrl;
            InitializeHttpClient();
        }
    }

    public static string GetBaseUrl() => _baseUrl;


    public static async Task<string> GetTokenAsync(
        int storeId, int posId, int cashierId, string username, string password,
        TimeSpan? timeout = null, CancellationToken callerToken = default)
    {
        var body = new
        {
            storeId,
            posId,
            cashierId,
            grant_type = "password",
            username,
            password
        };

        var json = Newtonsoft.Json.JsonConvert.SerializeObject(body);
         var content = new StringContent(json, Encoding.UTF8, "application/json-patch+json");
         var cts = CancellationTokenSource.CreateLinkedTokenSource(callerToken);
        cts.CancelAfter(timeout ?? TimeSpan.FromSeconds(30));

        var req = new HttpRequestMessage(HttpMethod.Post, "token") { Content = content, Version = new Version(1, 1) };
        HttpResponseMessage resp;
        try
        {
            resp = await _http.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, cts.Token)
                              .ConfigureAwait(false);
        }
        catch (OperationCanceledException oce)
        {
            if (callerToken.IsCancellationRequested)
                throw new TaskCanceledException("Token isteği çağıran tarafından iptal edildi.", oce);

            throw new TaskCanceledException("Token isteği zaman aşımına uğradı (muhtemel ağ/TLS/proxy).", oce);
        }

        var payload = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);
        if (!resp.IsSuccessStatusCode)
            throw new HttpRequestException($"Token request failed: {(int)resp.StatusCode} {resp.ReasonPhrase}. Body: {payload}");

        try
        {
            var s = Newtonsoft.Json.JsonConvert.DeserializeObject<string>(payload);
            if (!string.IsNullOrWhiteSpace(s)) return s;
        }
        catch { }
        dynamic obj = Newtonsoft.Json.JsonConvert.DeserializeObject(payload);
        return (string)(obj?.token ?? obj?.access_token ?? payload.Trim('"'));
    }
}
