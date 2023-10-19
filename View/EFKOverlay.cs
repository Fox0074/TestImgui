using ClickableTransparentOverlay;
using ClickableTransparentOverlay.Win32;
using DriverUserInterface.Structures;
using ImGuiNET;
using MiraEFK;
using System.Diagnostics;
using System.Numerics;
using UserInterfaceEFK.View;

namespace View
{
    internal class EFKOverlay : Overlay
    {
        internal static int Width = 1920;
        internal static int Height = 1080;

        private long _unityModule = 0;
        private bool _isActive = false;
        private bool _inGame = false;
        private bool isRunning = true;
        private bool _showSettings = true;
        private bool _isModuleInit = false;
        private string _bgName = T.RandomString(8);

        protected override Task PostInitialized()
        {
            Program.Style.InitStyle();

            SettingsView.Initialize();
            ReplaceFont("fixedsys.ttf", 15, FontGlyphRangeType.Cyrillic);
            LootEspOverlay.LootFilterText = string.Join("\n", Program.Config.LootFilter.ToArray());
            LootEspOverlay.RareFilterText = string.Join("\n", Program.Config.RareLootFilter.ToArray());

            return Task.CompletedTask;
        }

        protected override void Render()
        {
            try
            {
                Width = window.Dimensions.Width;
                Height = window.Dimensions.Height;

                if (!isRunning)
                    Close();

                CheckInput();

                if (Program.Config.ShowDebug)
                    DebugView.Render();

                if (_showSettings)
                    SettingsView.Render(ref _isActive);
            }
            catch (Exception ex)
            {
                Console.WriteLine("DrawLoop error");
                _isActive = false;
            }
        }


        private void CheckInput()
        {
            if (Utils.IsKeyPressedAndNotTimeout(VK.INSERT))
            {
                _showSettings = !_showSettings;
            }

            if (Utils.IsKeyPressedAndNotTimeout(Program.Config.BattleModeButton))
            {
                Program.Config.LootIsDraw = !Program.Config.LootIsDraw;
            }

            if (Utils.IsKeyPressedAndNotTimeout(VK.END))
            {
                isRunning = false;
                Program.SaveData();
                Environment.Exit(0);
            }
        }
    }
}
