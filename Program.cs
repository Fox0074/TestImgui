using ClickableTransparentOverlay;
using ClickableTransparentOverlay.Styles;
using DriverUserInterface.Structures;
using Newtonsoft.Json;
using System.Numerics;
using System.Text;
using View;

[assembly: ArmDot.Client.ObfuscateNames(Enable = true, Inherit = true)]
[assembly: ArmDot.Client.ObfuscateNamespaces(Enable = true)]
[assembly: ArmDot.Client.VirtualizeCode]
[assembly: ArmDot.Client.HideStrings]

namespace MiraEFK
{
    internal class Program
    {
        internal static string Game = "EFK";
        internal static bool IsValid;
        internal static Settings Config;
        internal static BaseStyle Style = new OrangeStyle();

        internal static string FolderPath { get; } = 
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
             "\\" + T.ConvertString("Mira") + "\\" + T.ConvertString("EFK");

        private static void Main(string[] args)
        {
            var selectedLang = Language.Ru;
            var x = ShowOverlay(selectedLang).Result;
        }

        private static async Task<bool> ShowOverlay(Language selectedLang)
        {
            try
            {
                Config = LoadData();
                Config.Lang = selectedLang;
                T.CurrentLang = Config.Lang;
                var overlay = new EFKOverlay();
                await overlay.Run();
            } catch (Exception)
            {
                Console.WriteLine("Error load config");
            }
            return true;
        }

        private static Settings LoadData()
        {
            using (FileStream fs = new FileStream(FolderPath + "\\sesionData", FileMode.OpenOrCreate))
            {
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                var loadedData = FromJson<Settings>(buffer);
                if (loadedData == null)
                    return new Settings();
                return loadedData;
            }
        }

        internal static void SaveData()
        {
            using (FileStream fs = new FileStream(FolderPath + "\\sesionData", FileMode.Create))
            {
                var jsonMessage = JsonConvert.SerializeObject(Program.Config);
                var byteData = Encoding.UTF8.GetBytes(jsonMessage);
                fs.Write(byteData, 0, byteData.Length);
            }
        }

        private static T FromJson<T>(byte[] data)
        {
            var jsonData = Encoding.UTF8.GetString(data);
            return JsonConvert.DeserializeObject<T>(jsonData);
        }
    }
}