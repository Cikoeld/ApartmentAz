using ApartmentAz.BLL.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json.Serialization;
using System.Net.Http.Json;

namespace ApartmentAz.BLL.Services;

public class TranslationService : ITranslationService
{
    private static readonly string[] SupportedLanguages = ["az", "en", "ru"];
    private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(24);
    private readonly HttpClient _http;
    private readonly IMemoryCache _cache;

    public TranslationService(HttpClient http, IMemoryCache cache)
    {
        _http = http;
        _cache = cache;
    }

    public async Task<string> TranslateAsync(string text, string fromLang, string toLang)
    {
        if (string.IsNullOrWhiteSpace(text) || fromLang == toLang)
            return text;

        var cacheKey = $"translate:{text}:{fromLang}:{toLang}";

        return await _cache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = CacheDuration;

            try
            {
                var encoded = Uri.EscapeDataString(text);
                var url = $"get?q={encoded}&langpair={fromLang}|{toLang}";

                var response = await _http.GetFromJsonAsync<MyMemoryResponse>(url);

                if (response?.ResponseStatus == 200
                    && !string.IsNullOrWhiteSpace(response.ResponseData?.TranslatedText))
                {
                    return response.ResponseData.TranslatedText;
                }
            }
            catch
            {
                // Translation failed — fall back to original text
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5); // short TTL on failure
            }

            return text;
        }) ?? text;
    }

    public async Task<Dictionary<string, string>> TranslateToAllAsync(string text, string sourceLang)
    {
        var result = new Dictionary<string, string> { [sourceLang] = text };

        if (string.IsNullOrWhiteSpace(text))
            return result;

        var tasks = SupportedLanguages
            .Where(lang => lang != sourceLang)
            .Select(async targetLang =>
            {
                var translated = await TranslateAsync(text, sourceLang, targetLang);
                return (targetLang, translated);
            });

        foreach (var (lang, translated) in await Task.WhenAll(tasks))
            result[lang] = translated;

        return result;
    }

    // MyMemory API response models
    private class MyMemoryResponse
    {
        [JsonPropertyName("responseData")]
        public MyMemoryResponseData? ResponseData { get; set; }

        [JsonPropertyName("responseStatus")]
        public int ResponseStatus { get; set; }
    }

    private class MyMemoryResponseData
    {
        [JsonPropertyName("translatedText")]
        public string? TranslatedText { get; set; }
    }
}
