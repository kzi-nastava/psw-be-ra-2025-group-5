using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.RateLimiting;

namespace Explorer.API.Controllers
{
    [Route("api/ai")]
    [ApiController]
    public class GenAIController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public GenAIController(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClient = httpClientFactory.CreateClient();
        }

        [EnableRateLimiting("ai-chat")]
        [HttpPost("chat")]
        public async Task<IActionResult> Chat([FromBody] ChatRequest request)
        {
            try
            {
                var apiKey = _configuration["Groq:ApiKey"];

                if (string.IsNullOrEmpty(apiKey))
                {
                    return StatusCode(500, new { text = "API key is not configured. Please register at https://console.groq.com and add your API key to appsettings.json under Groq:ApiKey" });
                }

                var groqRequest = new
                {
                    model = "llama-3.3-70b-versatile",
                    messages = new[]
                    {
                        new
                        {
                            role = "system",
                            content = "You are a helpful tour guide assistant. Provide concise, informative responses about tour locations, history, and keypoints."
                        },
                        new
                        {
                            role = "user",
                            content = request.Prompt
                        }
                    },
                    temperature = 0.7,
                    max_tokens = 500
                };

                var json = JsonSerializer.Serialize(groqRequest);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

                var response = await _httpClient.PostAsync(
                    "https://api.groq.com/openai/v1/chat/completions",
                    content
                );

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return StatusCode(500, new { text = $"Failed to get response from AI service: {errorContent}" });
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var groqResponse = JsonSerializer.Deserialize<GroqResponse>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                var textResponse = groqResponse?.Choices?.FirstOrDefault()?.Message?.Content
                    ?? "I couldn't generate a response.";

                return Ok(new { text = textResponse });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { text = $"Error: {ex.Message}" });
            }
        }
    }

    public class ChatRequest
    {
        public string Prompt { get; set; } = string.Empty;
    }

    public class GroqResponse
    {
        public List<GroqChoice>? Choices { get; set; }
    }

    public class GroqChoice
    {
        public GroqMessage? Message { get; set; }
    }

    public class GroqMessage
    {
        public string? Content { get; set; }
    }
}
