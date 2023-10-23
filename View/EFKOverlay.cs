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
        internal static int Width = 640;
        internal static int Height = 380;

        private long _unityModule = 0;
        private bool _isActive = false;
        private bool _inGame = false;
        private bool isRunning = true;
        private bool _showSettings = true;
        private bool _isModuleInit = false;
        private string _bgName = T.RandomString(8);
        private bool _btn1Clk = true;
        private bool _btn2Clk = false;
        private bool _btn3Clk = false;
        private int _TablesCount = 3;
        private int _splitLineStartXPos = 110;
        private int _splitLineWeight = 30;
        private int _tableButtonWeight = 100;
        private int _tableButtonHeight = 50;
        private int _subtableFieldWeight = 500;

        protected override Task PostInitialized()
        {
            Program.Style.InitStyle();

            SettingsView.Initialize();
            ReplaceFont("fixedsys.ttf", 15, FontGlyphRangeType.Cyrillic);
            LootEspOverlay.LootFilterText = string.Join("\n", Program.Config.LootFilter.ToArray());
            LootEspOverlay.RareFilterText = string.Join("\n", Program.Config.RareLootFilter.ToArray());

            return Task.CompletedTask;
        }

        protected void SwitchButton(int ButtonNum)
        {
            switch (ButtonNum)
            {
                case 1:
                    _btn1Clk = true;
                    _btn2Clk = false;
                    _btn3Clk = false;
                    break;
                case 2:
                    _btn1Clk = false;
                    _btn2Clk = true;
                    _btn3Clk = false;
                    break;
                case 3:
                    _btn1Clk = false;
                    _btn2Clk = false;
                    _btn3Clk = true;
                    break;
            }
        }
        protected void RenderSubTables(int ButtonNum)
        {
            ImGui.BeginTabBar(String.Format("Tabs{0}", ButtonNum), ImGuiTabBarFlags.AutoSelectNewTabs);

            if (ImGui.BeginTabItem(T.GetString(String.Format("Subtable {0}.1", ButtonNum))))
            {
                ImGui.SliderFloat(T.GetString("Дистанция отрисовки "), ref Program.Config.DrawDistance, 1, 2000, "%.0f");
                ImGui.Checkbox(T.GetString("Скелет"), ref Program.Config.DrawBones);
                ImGui.Checkbox(T.GetString("Рамка"), ref Program.Config.DrawBoxes);
                ImGui.Checkbox(T.GetString("Ник"), ref Program.Config.DrawNick);
                ImGui.SliderFloat(T.GetString("Масштаб радара"), ref Program.Config.RadarMashtab, 0f, 10f, "%.1f");
                ImGui.Checkbox(T.GetString("Жизни (HP)"), ref Program.Config.DrawHealth);
                ImGui.Checkbox(T.GetString("ID Команды"), ref Program.Config.ShowTemmates);
                ImGui.Checkbox(T.GetString("Оружие в руках"), ref Program.Config.ShowWeapon);
                ImGui.Checkbox(T.GetString("Выходы"), ref Program.Config.DrawExits);

                ImGui.EndTabItem();
            }

            if (ImGui.BeginTabItem(T.GetString(String.Format("Subtable {0}.2", ButtonNum))))
            {
                ImGui.Checkbox(T.GetString("Антиотдача"), ref Program.Config.NoRecoil);
                ImGui.Checkbox(T.GetString("Точка по центру экрана"), ref Program.Config.AimActive);
                ImGui.Checkbox(T.GetString("Радар"), ref Program.Config.DrawRadar);
                ImGui.SliderFloat(T.GetString("Позиция радара X"), ref Program.Config.RadarOffset.X, 0, -(EFKOverlay.Width - 100), "%1f");
                ImGui.SliderFloat(T.GetString("Позиция радара Y"), ref Program.Config.RadarOffset.Y, 0, EFKOverlay.Height - 100, "%1f");

                ImGui.Checkbox(T.GetString("Режим отладки"), ref Program.Config.ShowDebug);
                ImGui.EndTabItem();
            }

            if (ImGui.BeginTabItem(T.GetString(String.Format("Subtable {0}.3", ButtonNum))))
            {
                ImGui.Checkbox(T.GetString("Аимбот"), ref Program.Config.AimBot);

                if (Program.Config.AimTarget == AimTarget.Custom)
                    ImGui.DragFloat3(T.GetString("Свое смещение прицела"), ref Program.Config.AimCustomOffset, .1f);

                ImGui.SliderFloat(T.GetString("Радиус захвата"), ref Program.Config.AimFov, 5f, 500f, "%.1f");

                ImGui.EndTabItem();
            }
        }

        protected override void Render()
        {

            ImGui.SetWindowSize(new Vector2(Width, Height));
            ImGui.Begin(T.GetString("Slave"),
            ImGuiWindowFlags.NoBringToFrontOnFocus |
            ImGuiWindowFlags.NoCollapse |
            ImGuiWindowFlags.NoScrollbar |
            ImGuiWindowFlags.NoScrollWithMouse);
            ImGui.BeginChild("Vertical Bar with buttons", new Vector2(_tableButtonWeight, Height * 0.865f), false);

            for (int i = 1; i <= _TablesCount; i++)
                if (ImGui.Button(String.Format("Table {0}", i), new Vector2(_tableButtonWeight, _tableButtonHeight)))
                    SwitchButton(i);
            
            ImGui.EndChild();

            ImGui.SameLine(_splitLineStartXPos, _splitLineWeight);

            ImGui.BeginChild("Subtables", new Vector2(_subtableFieldWeight, Height * 0.865f), false);
            if (_btn1Clk)
                RenderSubTables(1);
                
            if (_btn2Clk)
                RenderSubTables(2);

            if (_btn3Clk)
                RenderSubTables(3);

            ImGui.EndChild();

            ImGui.End();
        }
    }
}
