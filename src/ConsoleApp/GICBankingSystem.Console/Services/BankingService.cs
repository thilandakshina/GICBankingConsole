using System.Text;
using System.Text.Json;
using GICBankingSystem.Console.Models;

namespace GICBankingSystem.Console.Services
{
    public class BankingService : IBankingService
    {
        private readonly HttpClient _client;
        private const string BaseUrl = "https://localhost:5051/api/";

        public BankingService()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            _client = new HttpClient(handler)
            {
                BaseAddress = new Uri(BaseUrl)
            };
        }

        public async Task<TransactionResponse> ProcessTransactionAsync(TransactionRequest request)
        {
            var response = await SendRequestAsync<TransactionResponse>("Transaction", HttpMethod.Post, request);
            return response;
        }

        public async Task<List<InterestRule>> AddInterestRuleAsync(InterestRuleRequest request)
        {
            var response = await SendRequestAsync<List<InterestRule>>("Interest", HttpMethod.Post, request);
            return response;
        }

        public async Task<StatementResponse> GetStatementAsync(string accountNo, string period)
        {
            var response = await SendRequestAsync<StatementResponse>($"Statement?accountNo={accountNo}&period={period}", HttpMethod.Get);
            return response;
        }

        private async Task<T> SendRequestAsync<T>(string endpoint, HttpMethod method, object content = null)
        {
            try
            {
                HttpResponseMessage response;
                if (method == HttpMethod.Get)
                {
                    response = await _client.GetAsync(endpoint);
                }
                else
                {
                    var jsonContent = new StringContent(
                        JsonSerializer.Serialize(content, new JsonSerializerOptions
                        {
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                            WriteIndented = true
                        }),
                        Encoding.UTF8,
                        "application/json"
                    );
                    response = await _client.PostAsync(endpoint, jsonContent);
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(
                responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

                    throw new HttpRequestException(
                        $"Error: {errorResponse.Detail}\n" +
                        $"TraceId: {errorResponse.TraceId}"
                    );
                }

                return JsonSerializer.Deserialize<T>(
                    responseContent,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );
            }
            catch (Exception ex)
            {
                throw new HttpRequestException($"{ex.Message}");
            }
        }
    }
}
