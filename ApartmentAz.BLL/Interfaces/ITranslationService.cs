namespace ApartmentAz.BLL.Interfaces;

public interface ITranslationService
{
    Task<string> TranslateAsync(string text, string fromLang, string toLang);
    Task<Dictionary<string, string>> TranslateToAllAsync(string text, string sourceLang);
}
