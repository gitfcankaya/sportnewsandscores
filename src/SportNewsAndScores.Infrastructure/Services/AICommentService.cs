using Microsoft.Extensions.Configuration;
using SportNewsAndScores.Core.Interfaces;
using System.Net.Http.Json;
using System.Text.Json;

namespace SportNewsAndScores.Infrastructure.Services;

public class AICommentService : IAICommentService
{
    private readonly HttpClient _httpClient;
    private readonly string _openAIKey;
    private readonly string _geminiKey;

    public AICommentService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _openAIKey = configuration["OpenAI:ApiKey"] ?? "";
        _geminiKey = configuration["Gemini:ApiKey"] ?? "";
    }

    public async Task<string> GenerateCommentAsync(string content, string provider)
    {
        if (provider.ToLower() == "openai")
        {
            return await GenerateOpenAICommentAsync(content);
        }
        else if (provider.ToLower() == "gemini")
        {
            return await GenerateGeminiCommentAsync(content);
        }
        
        return "No AI provider specified";
    }

    private async Task<string> GenerateOpenAICommentAsync(string content)
    {
        if (string.IsNullOrEmpty(_openAIKey))
        {
            return $"AI Commentary: {content} - This is a simulated OpenAI response. Configure OpenAI API key to get real AI-generated commentary.";
        }

        try
        {
            var requestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                    new { role = "system", content = "You are a sports commentator. Provide brief, engaging commentary on sports news." },
                    new { role = "user", content = $"Provide commentary on: {content}" }
                },
                max_tokens = 150
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions")
            {
                Content = JsonContent.Create(requestBody)
            };
            request.Headers.Add("Authorization", $"Bearer {_openAIKey}");
            
            var response = await _httpClient.SendAsync(request);
            
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<JsonDocument>();
                return result?.RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString() ?? "No commentary generated";
            }
        }
        catch (Exception ex)
        {
            // TODO: Implement proper logging
            Console.WriteLine($"Error generating OpenAI comment: {ex.Message}");
        }

        return $"AI Commentary: {content} - This is a simulated response as the API call failed.";
    }

    private async Task<string> GenerateGeminiCommentAsync(string content)
    {
        if (string.IsNullOrEmpty(_geminiKey))
        {
            return $"AI Commentary (Gemini): {content} - This is a simulated Gemini response. Configure Gemini API key to get real AI-generated commentary.";
        }

        try
        {
            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = $"You are a sports commentator. Provide brief, engaging commentary on: {content}" }
                        }
                    }
                }
            };

            var response = await _httpClient.PostAsJsonAsync(
                $"https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent?key={_geminiKey}", 
                requestBody);
            
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<JsonDocument>();
                return result?.RootElement
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString() ?? "No commentary generated";
            }
        }
        catch (Exception ex)
        {
            // TODO: Implement proper logging
            Console.WriteLine($"Error generating Gemini comment: {ex.Message}");
        }

        return $"AI Commentary (Gemini): {content} - This is a simulated response as the API call failed.";
    }
}
