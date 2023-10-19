using ClickableTransparentOverlay;
using ClickableTransparentOverlay.Win32;
using DriverUserInterface.Structures;
using ImGuiNET;
using MiraEFK;
using View;

namespace UserInterfaceEFK.View
{
    internal class SettingsView
    {
        private static int _aimTarget = 0;
        private static int _battleMod = 0;
        private static int _aimButton = 0;

        internal static void Initialize()
        {
            _aimTarget = (int)Program.Config.AimTarget;
            _battleMod = Program.Config.BattleModeButton == VK.NUMPAD0 ? 0 : 1;
            switch (Program.Config.AimButton)
            {
                case VK.LBUTTON:
                    _aimButton = 0;
                    break;
                case VK.RBUTTON:
                    _aimButton = 1;
                    break;
                case VK.MENU:
                    _aimButton = 2;
                    break;
            }
        }

        [ArmDot.Client.VirtualizeCode(Enable = false, Inherit = true)]
        internal static void Render(ref bool isActive)
        {
            ImGui.Begin(T.GetString("Settings"),
                ImGuiWindowFlags.AlwaysAutoResize |
                ImGuiWindowFlags.NoBringToFrontOnFocus |
                ImGuiWindowFlags.NoCollapse);

            if (!isActive)
            {
                if (ImGui.Button(T.GetString("Старт")))
                {
                    isActive = !isActive;
                }
            }

            /*if (!isActive)
            {
                if (ImGui.Button(T.GetString("Стоп")))
                {
                    isActive = !isActive;
                }
            }*/

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

                    if (ImGui.SliderFloat(T.GetString("Дистанция отрисовки "), ref Program.Config.DrawDistance, 1, 2000, "%.0f"))
                        Program.SaveData();
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
                    if (ImGui.Combo(T.GetString("Боевой режим"), ref _battleMod,
                        new string[] { T.GetString("Num 0"),
                            T.GetString("F3") }, 2))
                    {
                        Program.Config.BattleModeButton = _battleMod == 0 ? VK.NUMPAD0 : VK.F3;
                        Program.SaveData();
                    }
                    
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

                    if (ImGui.Combo(T.GetString("Точка прицеливания"), ref _aimTarget,
                        new string[] { T.GetString("Свой"),
                            T.GetString("Голова"), T.GetString("П.Рука"),
                            T.GetString("Л.Рука"), T.GetString("Л.Нога"),
                            T.GetString("П.Нога"), T.GetString("Грудь"),
                            T.GetString("Живот") }, 8))
                    {
                        Program.Config.AimTarget = (AimTarget)_aimTarget;
                        Program.SaveData();
                    }

                    if (Program.Config.AimTarget == AimTarget.Custom)
                    {
                        if (ImGui.DragFloat3(T.GetString("Свое смещение прицела"), ref Program.Config.AimCustomOffset, .1f))
                            Program.SaveData();
                    }

                    if (ImGui.Combo(T.GetString("Кнопка для аима"), ref _aimButton, new string[] { T.GetString("ЛКМ"), T.GetString("ПКМ"), T.GetString("ALT"), T.GetString("CTRL") }, 4))
                    {
                        switch (_aimButton)
                        {
                            case 0:
                                Program.Config.AimButton = VK.LBUTTON;
                                break;
                            case 1:
                                Program.Config.AimButton = VK.RBUTTON;
                                break;
                            case 2:
                                Program.Config.AimButton = VK.MENU;
                                break;
                            case 3:
                                Program.Config.AimButton = VK.CONTROL;
                                break;
                        }

                        Program.SaveData();
                    }

                    if (ImGui.SliderFloat(T.GetString("Радиус захвата (Aim FOV)"), ref Program.Config.AimFov, 5f, 500f, "%.1f"))
                        Program.SaveData();

                    ImGui.EndTabItem();
                }

                if (ImGui.BeginTabItem(T.GetString("Цвета")))
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
                }
            }

            ImGui.End();
        }
    }
}
