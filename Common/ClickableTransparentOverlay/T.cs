using Newtonsoft.Json;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace ClickableTransparentOverlay
{
    public enum Language { None, Ru, En }
    internal class T
    {
        internal static Language CurrentLang = Language.Ru;


        [ArmDot.Client.ObfuscateNames(Enable = false)]
        private class Phrase
        {
            internal string Ru;
            internal string En;
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        internal static string XorString(string key, string input)
        {
            return XorString(key, input, input.Length);
        }

        internal static string XorString(string key, string input, int length)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++)
                sb.Append((char)(input[i] ^ key[(i % key.Length)]));
            String result = sb.ToString();

            return result;
        }

        internal static string GetString(string ru)
        {
            var phrase = Phrases.FirstOrDefault(x => x.Ru == ru);

            if (phrase == null)
                return ru;

            switch (CurrentLang)
            {
                case Language.Ru:
                    return ru;

                case Language.En:
                    return phrase.En;

                default:
                    return phrase.Ru;
            }
        }

        internal static string ConvertString(string s)
        {
            return System.Convert.ToBase64String(Encoding.UTF8.GetBytes(s)).Replace('\\', 'a')
                .Replace('/', 'b').Replace(':', 'c')
                .Replace('*', 'd').Replace('?', 'e')
                .Replace('"', 'f').Replace('<', 'g')
                .Replace('>', 'r').Replace('|', 'j');
        }

        private static List<Phrase> Phrases = new List<Phrase>()
        {
            //Tundra
            new Phrase() { Ru = "Авторизация", En = "Authorization"},
            new Phrase() { Ru = "Ключ", En = "Key"},
            new Phrase() { Ru = "Старт", En = "Start"},
            new Phrase() { Ru = "Функции", En = "Functions"},
            new Phrase() { Ru = "Рисовать коробки", En = "Draw Boxes"},
            new Phrase() { Ru = "Рисовать орудие", En = "Draw Weapon"},
            new Phrase() { Ru = "Точка упреждения", En = "Lead point"},
            new Phrase() { Ru = "Ники", En = "Show Nicks"},
        };
    }
}
