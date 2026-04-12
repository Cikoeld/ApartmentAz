using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace ApartmentAz.CLIENT.Services;

public class ClientTranslationService
{
    private static readonly string[] SupportedLanguages = ["az", "en", "ru"];
    private readonly HttpClient _http;

    public ClientTranslationService(HttpClient http) => _http = http;

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

    private async Task<string> TranslateAsync(string text, string fromLang, string toLang)
    {
        if (string.IsNullOrWhiteSpace(text) || fromLang == toLang)
            return text;

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
        }

        return text;
    }

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
