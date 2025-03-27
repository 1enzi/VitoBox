using System.Net.Http.Headers;
using System.Text.Json;
using VitoBox.Constants;
using VitoBox.Models;
using VitoBox.Models.Enums;
using VitoBox.Models.Interfaces;
using VitoBox.Runtime;
using VitoBox.Utils;

namespace VitoBox.Infrastructure;

public class VitoApiService(string apiUrl, string apiKey, string model) : IVitoApiService
{
    private readonly HttpClient _http = new();
    private readonly string _apiKey = apiKey;
    private readonly string _apiUrl = apiUrl;
    private readonly string _model = model;

    public async Task<VitoResponse> GetVitoResponseAsync(string prompt, CancellationToken token)
    {
        if (IsArchiveWhisper())
            return GetWhisperedResponse();

        try
        {
            var json = await SendRequestAsync(prompt, token);
            var replyText = ExtractContentFromResponse(json);
            return ParseReplyOrFail(replyText);
        }
        catch (HttpRequestException ex)
        {
            Logger.LogError($"OpenAI API error: {ex.Message}");
            return Fail(VitoPhrases.ApiErrorFormat, VitoSounds.ErrorApi);
        }
        catch (Exception ex)
        {
            Logger.LogError($"API exception: {ex.Message}");
            return Fail(VitoPhrases.ApiException, VitoSounds.ErrorException);
        }
    }

    private async Task<string> SendRequestAsync(string prompt, CancellationToken token)
    {
        var requestBody = VitoChatRequest.FromPrompt(prompt, _model);
        var content = new StringContent(JsonSerializer.Serialize(requestBody));
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

        var response = await _http.PostAsync(_apiUrl, content, token);
        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException($"Status: {response.StatusCode} — {await response.Content.ReadAsStringAsync()}");

        return await response.Content.ReadAsStringAsync(token);
    }


    private VitoResponse ParseReplyOrFail(string? raw)
    {
        var vito = JsonSerializer.Deserialize<VitoResponse>(raw ?? "");
        return vito ?? Fail(VitoPhrases.ParseError, VitoSounds.ErrorParse);
    }

    private VitoResponse Fail(string textOrFormat, string sound)
    {
        var text = textOrFormat.Contains("{0}")
            ? string.Format(textOrFormat, "неизвестно")
            : textOrFormat;

        return new VitoResponse
        {
            Text = text,
            Sound = sound,
            Vibration = true,
        };
    }

    private bool IsArchiveWhisper() => VitoMemory.State == VitoState.ArchiveWhisper;

    private VitoResponse GetWhisperedResponse()
    {
        return new VitoResponse
        {
            Text = VitoPhrases.ArchiveMurmurs[Random.Shared.Next(VitoPhrases.ArchiveMurmurs.Length)],
            Sound = VitoSounds.ArchiveWhisper,
            Vibration = true,
        };
    }

    private string? ExtractContentFromResponse(string json)
    {
        try
        {
            return JsonDocument.Parse(json)
                .RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();
        }
        catch
        {
            return null;
        }
    }
}