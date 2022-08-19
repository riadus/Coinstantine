using System.Collections.Generic;
using Coinstantine.Common.Attributes;

namespace Coinstantine.Core.Fonts
{
    [RegisterInterfaceAsLazySingleton]
    public class CodeFontProvider : ICodeFontProvider
    {
        private Dictionary<string, (string, FontType)> _codes;
        private Dictionary<string, AppColorDefinition> _colors;

        public (string Code, FontType Font) GetCode(string text)
        {
            if (Codes.ContainsKey(text))
            {
                return Codes[text];
            }
            return (text, FontType.SanFrancisco);
        }

        private Dictionary<string, (string, FontType)> Codes => _codes ?? (_codes = BuildDictionary());
        private Dictionary<string, AppColorDefinition> Colors => _colors ?? (_colors = BuildColorDictionary());

        public bool Contains(string code)
        {
            return Codes.ContainsKey(code);
        }

        public (bool ColorApplies, AppColorDefinition Color) GetColor(string code)
        {
            if(Colors.ContainsKey(code))
            {
                return (true, Colors[code]);
            }
            return (false, AppColorDefinition.Default);
        }

        private static Dictionary<string, AppColorDefinition> BuildColorDictionary()
        {
            return new Dictionary<string, AppColorDefinition>
            {
                {"True", AppColorDefinition.SecondaryColor},
                {"False", AppColorDefinition.Error},
                {"denied", AppColorDefinition.Error},
                {"subscribed", AppColorDefinition.MainBlue},
                {"validated", AppColorDefinition.MainBlue},
                {"withdrawn", AppColorDefinition.MainBlue},
                {"airdroped", AppColorDefinition.MainBlue}
            };
        }

        private static Dictionary<string, (string, FontType)> BuildDictionary()
        {
            return new Dictionary<string, (string, FontType)>
            {
                { "ellipsis-v", ("\uf142", FontType.FontAwesomeSolidSupport) },
                { "sync-alt", ("\uf2f1", FontType.FontAwesomeSolid) },
                { "chevron-up", ("\uf077", FontType.FontAwesomeSolid)},
                { "exclamation-circle", ("\uf06a", FontType.FontAwesomeSolid)},
                { "wallet", ("\uf555", FontType.FontAwesomeSolid)},
                { "sign-out-alt", ("\uf2f5", FontType.FontAwesomeSolid)},
                { "user", ("\uf007", FontType.FontAwesomeSolid)},
                { "cog", ("\uf013", FontType.FontAwesomeSolid)},
                { "lock", ("\uf023", FontType.FontAwesomeSolid)},
                { "unlock-alt", ("\uf13e", FontType.FontAwesomeSolid)},
                { "arrow-circle-right", ("\uf0da", FontType.FontAwesomeSolid)},
                { "arrow-circle-left", ("\uf0d9", FontType.FontAwesomeSolid)},
                { "caret-up", ("\uf0d8", FontType.FontAwesomeSolid)},
                { "caret-down", ("\uf0d9", FontType.FontAwesomeSolid)},
                { "at", ("\uf1fa", FontType.FontAwesomeSolid)},
                { "calendar-alt", ("\uf073", FontType.FontAwesomeSolid)},
                { "address-card", ("\uf2bb", FontType.FontAwesomeSolid)},
                { "mail-bulk", ("\uf674", FontType.FontAwesomeSolid)},

                { "laptop", ("\uf109", FontType.FontAwesomeSolid)},
                { "shopping-cart", ("\uf07a", FontType.FontAwesomeSolid)},
                { "affiliatetheme", ("\uf36b", FontType.FontAwesomeBrand)},
                { "ethereum", ("\uf42e", FontType.FontAwesomeBrand)},
                { "flag", ("\uf024", FontType.FontAwesomeSolid)},
                { "undo", ("\uf0e2", FontType.FontAwesomeSolid)},
                { "comment", ("\uf075", FontType.FontAwesomeSolid)},
                { "file-alt", ("\uf15c", FontType.FontAwesomeSolid)},
                { "share-square", ("\uf14d", FontType.FontAwesomeSolid)},
                { "twitter", ("\uf099", FontType.FontAwesomeBrand)},
                { "telegram-plane", ("\uf3fe", FontType.FontAwesomeBrand)},
                { "telegram", ("\uf2c6", FontType.FontAwesomeBrandNegative)},
                { "bitcoin", ("\uf379", FontType.FontAwesomeBrandNegative)},
                { "arrow-alt-circle-left", ("\uf359", FontType.FontAwesomeRegular)},
                { "check", ("\uf00c", FontType.FontAwesomeSolid)},
                { "True", ("\uf00c", FontType.FontAwesomeSolid)},
                { "False", ("\uf111", FontType.FontAwesomeSolid)},

                { "about", ("\ue900", FontType.IcoMoon)},
                { "airdrop", ("\ue901", FontType.IcoMoon)},
                { "airdroped", ("\ue902", FontType.IcoMoon)},
                { "C", ("\ue903", FontType.IcoMoon)},
                { "c", ("\ue903", FontType.IcoMoon)},
                { "denied", ("\ue904", FontType.IcoMoon)},
                { "fingerprint-alt", ("\ue905", FontType.IcoMoon)},
                { "fingerprint", ("\ue906", FontType.IcoMoon)},
                { "subscribed", ("\ue907", FontType.IcoMoon)},
                { "validated", ("\ue908", FontType.IcoMoon)},
                { "withdrawn", ("\ue909", FontType.IcoMoon)},
            };
        }
    }
}
