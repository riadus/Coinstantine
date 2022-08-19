namespace Coinstantine.Core.Fonts
{
    public interface ICodeFontProvider
    {
        (string Code, FontType Font) GetCode(string text);
        bool Contains(string code);
        (bool ColorApplies, AppColorDefinition Color) GetColor(string code);
    }
}
