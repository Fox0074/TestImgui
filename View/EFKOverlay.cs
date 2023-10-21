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
        internal static int Width = 520;
        internal static int Height = 410;

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
            ImGui.SetNextWindowSize(new Vector2(Width, Height));
            ImGui.Begin(T.GetString("Slave"),
                //ImGuiWindowFlags.AlwaysAutoResize |
                ImGuiWindowFlags.NoBringToFrontOnFocus |
                ImGuiWindowFlags.NoCollapse);

            if (ImGui.BeginTabBar("Tabs", ImGuiTabBarFlags.AutoSelectNewTabs))
            {
                if (ImGui.BeginTabItem(T.GetString("ЕСП")))
                {
                    if (Program.Config.UseExperimentalVersion)
                    {
                        if (ImGui.Button(T.GetString("Показывать отладку")))
                        {
                            Program.Config.ShowDebug = !Program.Config.ShowDebug;
                            Program.SaveData();
                        }
                    }

                    ImGui.SliderFloat(T.GetString("Дистанция отрисовки "), ref Program.Config.DrawDistance, 1, 2000, "%.0f");
                    if (ImGui.Checkbox(T.GetString("Скелет"), ref Program.Config.DrawBones))
                        Program.SaveData();
                    if (ImGui.Checkbox(T.GetString("Рамка"), ref Program.Config.DrawBoxes))
                        Program.SaveData();
                    if (ImGui.Checkbox(T.GetString("Ник"), ref Program.Config.DrawNick))
                        Program.SaveData();
                    if (ImGui.SliderFloat(T.GetString("Масштаб радара"), ref Program.Config.RadarMashtab, 0f, 10f, "%.1f"))
                        Program.SaveData();
                    if (ImGui.Checkbox(T.GetString("Жизни (HP)"), ref Program.Config.DrawHealth))
                        Program.SaveData();
                    if (ImGui.Checkbox(T.GetString("ID Команды"), ref Program.Config.ShowTemmates))
                        Program.SaveData();
                    if (ImGui.Checkbox(T.GetString("Оружие в руках"), ref Program.Config.ShowWeapon))
                        Program.SaveData();
                    if (ImGui.Checkbox(T.GetString("Выходы"), ref Program.Config.DrawExits))
                        Program.SaveData();

                    if (ImGui.Checkbox(T.GetString("Тень под текстом"), ref Program.Config.DrawShadows))
                        Program.SaveData();

                    ImGui.EndTabItem();
                }

                if (ImGui.BeginTabItem(T.GetString("Разное")))
                {
                    if (ImGui.Checkbox(T.GetString("Антиотдача"), ref Program.Config.NoRecoil))
                        Program.SaveData();
                    //if (ImGui.Checkbox(T.GetString("Бесконечная выносливость"), ref Program.Config.Stamina))
                    //    Program.SaveData();
                    if (ImGui.Checkbox(T.GetString("Точка по центру экрана"), ref Program.Config.AimActive))
                        Program.SaveData();
                    if (ImGui.Checkbox(T.GetString("Радар"), ref Program.Config.DrawRadar))
                        Program.SaveData();
                    if (ImGui.SliderFloat(T.GetString("Позиция радара X"), ref Program.Config.RadarOffset.X, 0, -(EFKOverlay.Width - 100), "%1f"))
                        Program.SaveData();
                    if (ImGui.SliderFloat(T.GetString("Позиция радара Y"), ref Program.Config.RadarOffset.Y, 0, EFKOverlay.Height - 100, "%1f"))
                        Program.SaveData();
                   

                    if (ImGui.Checkbox(T.GetString("Режим отладки"), ref Program.Config.ShowDebug))
                        Program.SaveData();
                    ImGui.EndTabItem();
                }

                if (ImGui.BeginTabItem(T.GetString("Лут")))
                {
                    LootEspOverlay.Render();

                    ImGui.EndTabItem();
                }

                if (ImGui.BeginTabItem(T.GetString("Аим")))
                {

                    if (ImGui.Checkbox(T.GetString("Аимбот"), ref Program.Config.AimBot))
                        Program.SaveData();

                 

                    if (Program.Config.AimTarget == AimTarget.Custom)
                    {
                        if (ImGui.DragFloat3(T.GetString("Свое смещение прицела"), ref Program.Config.AimCustomOffset, .1f))
                            Program.SaveData();
                    }


                    if (ImGui.SliderFloat(T.GetString("Радиус захвата (Aim FOV)"), ref Program.Config.AimFov, 5f, 500f, "%.1f"))
                        Program.SaveData();

                    ImGui.EndTabItem();
                }

                /*if (ImGui.BeginTabItem(T.GetString("Цвета")))
                {
                    if (ImGui.ColorEdit4(T.GetString("Usec"), ref Program.Config.UsecColor))
                        Program.SaveData();
                    if (ImGui.ColorEdit4(T.GetString("Bear"), ref Program.Config.BearColor))
                        Program.SaveData();
                    if (ImGui.ColorEdit4(T.GetString("Дикий"), ref Program.Config.BotPlayersColor))
                        Program.SaveData();
                    if (ImGui.ColorEdit4(T.GetString("Лут"), ref Program.Config.LootColor))
                        Program.SaveData();
                    if (ImGui.ColorEdit4(T.GetString("Редкий лут"), ref Program.Config.RareLootColor))
                        Program.SaveData();
                    if (ImGui.ColorEdit4(T.GetString("Трупы"), ref Program.Config.CorpseColor))
                        Program.SaveData();
                    if (ImGui.ColorEdit4(T.GetString("Контейнеры"), ref Program.Config.ContainerColor))
                        Program.SaveData();
                    if (ImGui.ColorEdit4(T.GetString("Бот"), ref Program.Config.BotColor))
                        Program.SaveData();
                    if (ImGui.ColorEdit4(T.GetString("Босы"), ref Program.Config.BossColor))
                        Program.SaveData();
                    if (ImGui.ColorEdit4(T.GetString("Свита"), ref Program.Config.SvitaColor))
                        Program.SaveData();
                    if (ImGui.ColorEdit4(T.GetString("Выходы"), ref Program.Config.ExitColor))
                        Program.SaveData();
                    if (ImGui.ColorEdit4(T.GetString("Другое"), ref Program.Config.DefaultColor))
                        Program.SaveData();

                    ImGui.EndTabItem();
                }*/
            }

            ImGui.End();
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
