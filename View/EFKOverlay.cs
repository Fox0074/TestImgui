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
        bool btn1_clk = false;
        bool btn2_clk = false;
        bool btn3_clk = false;

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

                ImGui.SetNextWindowSize(new Vector2(Width, Height));
                ImGui.Begin(T.GetString("Slave"),
                //ImGuiWindowFlags.AlwaysAutoResize |
                ImGuiWindowFlags.NoBringToFrontOnFocus |
                ImGuiWindowFlags.NoCollapse |
                ImGuiWindowFlags.NoScrollbar |
                ImGuiWindowFlags.NoScrollWithMouse);

            ImGui.BeginChild("Vertical Bar with buttons", new Vector2(100, Height * 0.865f), false);


            if (ImGui.Button("Table 1", new Vector2(100, 50)))
            {
                btn1_clk = true;
                btn2_clk = false;
                btn3_clk = false;
            }
            if (ImGui.Button("Table 2", new Vector2(100, 50)))
            {
                btn2_clk = true;
                btn1_clk = false;
                btn3_clk = false;
            }
            if (ImGui.Button("Table 3", new Vector2(100, 50)))
            {
                btn3_clk = true;
                btn1_clk = false;
                btn2_clk = false;
            }
                ImGui.EndChild();

                ImGui.SameLine(110, 30);

                ImGui.BeginChild("Subtables", new Vector2(500, Height * 0.865f), false);
                if (btn1_clk)
                {
                    ImGui.BeginTabBar("Tabs1", ImGuiTabBarFlags.AutoSelectNewTabs);

                    if (ImGui.BeginTabItem(T.GetString("Subtable 1.1")))
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

                    if (ImGui.BeginTabItem(T.GetString("Subtable 1.2")))
                    {
                        ImGui.Checkbox(T.GetString("Антиотдача"), ref Program.Config.NoRecoil);
                        ImGui.Checkbox(T.GetString("Точка по центру экрана"), ref Program.Config.AimActive);
                        ImGui.Checkbox(T.GetString("Радар"), ref Program.Config.DrawRadar);
                        ImGui.SliderFloat(T.GetString("Позиция радара X"), ref Program.Config.RadarOffset.X, 0, -(EFKOverlay.Width - 100), "%1f");
                        ImGui.SliderFloat(T.GetString("Позиция радара Y"), ref Program.Config.RadarOffset.Y, 0, EFKOverlay.Height - 100, "%1f");

                        ImGui.Checkbox(T.GetString("Режим отладки"), ref Program.Config.ShowDebug);
                        ImGui.EndTabItem();
                    }

                    if (ImGui.BeginTabItem(T.GetString("Subtable 1.3")))
                    {
                        ImGui.Checkbox(T.GetString("Аимбот"), ref Program.Config.AimBot);

                        if (Program.Config.AimTarget == AimTarget.Custom)
                            ImGui.DragFloat3(T.GetString("Свое смещение прицела"), ref Program.Config.AimCustomOffset, .1f);

                        ImGui.SliderFloat(T.GetString("Радиус захвата (Aim FOV)"), ref Program.Config.AimFov, 5f, 500f, "%.1f");

                        ImGui.EndTabItem();
                    }
                    ImGui.EndTabBar();
                }
                if (btn2_clk)
                {
                    ImGui.BeginTabBar("Tabs2", ImGuiTabBarFlags.AutoSelectNewTabs);
                    if (ImGui.BeginTabItem(T.GetString("Subtable 2.1")))
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

                    if (ImGui.BeginTabItem(T.GetString("Subtable 2.2")))
                    {
                        ImGui.Checkbox(T.GetString("Антиотдача"), ref Program.Config.NoRecoil);
                        ImGui.Checkbox(T.GetString("Точка по центру экрана"), ref Program.Config.AimActive);
                        ImGui.Checkbox(T.GetString("Радар"), ref Program.Config.DrawRadar);
                        ImGui.SliderFloat(T.GetString("Позиция радара X"), ref Program.Config.RadarOffset.X, 0, -(EFKOverlay.Width - 100), "%1f");
                        ImGui.SliderFloat(T.GetString("Позиция радара Y"), ref Program.Config.RadarOffset.Y, 0, EFKOverlay.Height - 100, "%1f");

                        ImGui.Checkbox(T.GetString("Режим отладки"), ref Program.Config.ShowDebug);
                        ImGui.EndTabItem();
                    }

                    if (ImGui.BeginTabItem(T.GetString("Subtable 2.3")))
                    {
                        ImGui.Checkbox(T.GetString("Аимбот"), ref Program.Config.AimBot);

                        if (Program.Config.AimTarget == AimTarget.Custom)
                            ImGui.DragFloat3(T.GetString("Свое смещение прицела"), ref Program.Config.AimCustomOffset, .1f);

                        ImGui.SliderFloat(T.GetString("Радиус захвата (Aim FOV)"), ref Program.Config.AimFov, 5f, 500f, "%.1f");

                        ImGui.EndTabItem();
                    }
                    ImGui.EndTabBar();
                }

                if (btn3_clk)
                {
                    ImGui.BeginTabBar("Tabs3", ImGuiTabBarFlags.AutoSelectNewTabs);
                    if (ImGui.BeginTabItem(T.GetString("Subtable 3.1")))
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

                    if (ImGui.BeginTabItem(T.GetString("Subtable 3.2")))
                    {
                        ImGui.Checkbox(T.GetString("Антиотдача"), ref Program.Config.NoRecoil);
                        ImGui.Checkbox(T.GetString("Точка по центру экрана"), ref Program.Config.AimActive);
                        ImGui.Checkbox(T.GetString("Радар"), ref Program.Config.DrawRadar);
                        ImGui.SliderFloat(T.GetString("Позиция радара X"), ref Program.Config.RadarOffset.X, 0, -(EFKOverlay.Width - 100), "%1f");
                        ImGui.SliderFloat(T.GetString("Позиция радара Y"), ref Program.Config.RadarOffset.Y, 0, EFKOverlay.Height - 100, "%1f");

                        ImGui.Checkbox(T.GetString("Режим отладки"), ref Program.Config.ShowDebug);
                        ImGui.EndTabItem();
                    }

                    if (ImGui.BeginTabItem(T.GetString("Subtable 3.3")))
                    {
                        ImGui.Checkbox(T.GetString("Аимбот"), ref Program.Config.AimBot);

                        if (Program.Config.AimTarget == AimTarget.Custom)
                            ImGui.DragFloat3(T.GetString("Свое смещение прицела"), ref Program.Config.AimCustomOffset, .1f);

                        ImGui.SliderFloat(T.GetString("Радиус захвата (Aim FOV)"), ref Program.Config.AimFov, 5f, 500f, "%.1f");

                        ImGui.EndTabItem();
                    }

                    ImGui.EndTabBar();
                }
                ImGui.EndChild();

                ImGui.End();
        }
    }
}
