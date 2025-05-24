using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BaseApiReference.Abstractions.Payments;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Infrastructure.Payment.VietQr;

/// <summary>
/// Implementation of sending mail handler interface.
/// </summary>
public class VietQRCodeHandler : IQRCodeHandler
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;
    private const string VIETQR_BASE_URL = "https://api.vietqr.io/v2/generate";

    public VietQRCodeHandler(IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        _configuration = configuration;
        _httpClient = httpClientFactory.CreateClient();
    }

    public async Task<string> CreateUrlQRCode(dynamic qRCodeModel)
    {
        qRCodeModel = qRCodeModel as QRCodeModel;
        var clientId = _configuration["VietQR:ClientId"];
        var apiKey = _configuration["VietQR:ApiKey"];
        var vietQRModel = new
        {
            accountNo = qRCodeModel.AccountNo,
            accountName = qRCodeModel.AccountName,
            acqId = qRCodeModel.AcqId,
            amount = qRCodeModel.Amount,
            addInfo = qRCodeModel.AddInfo,
            format = "text",
            template = "compact"
        };

        var requestBody = new StringContent(
            JsonConvert.SerializeObject(vietQRModel),
            Encoding.UTF8,
            "application/json"
        );

        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("x-client-id", clientId);
        _httpClient.DefaultRequestHeaders.Add("x-api-key", apiKey);

        var response = await _httpClient.PostAsync(VIETQR_BASE_URL, requestBody);

        if (response.IsSuccessStatusCode)
        {
            var responseData = await response.Content.ReadAsStringAsync();

            var vietQRResponse = JsonConvert.DeserializeObject<VietQRResponseAPI>(responseData);

            return vietQRResponse?.Data?.QrDataURL;
        }

        return default;
    }

    public sealed class VietQRResponseAPI
    {
        public string Code { get; set; }
        public string Desc { get; set; }
        public DataOption Data { get; set; }

        public class DataOption
        {
            public int AcpId { get; set; }
            public string AccountName { get; set; }
            public string QrCode { get; set; }
            public string QrDataURL { get; set; }
        }
    }
}
