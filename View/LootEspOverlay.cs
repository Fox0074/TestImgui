using ImGuiNET;
using MiraEFK;

namespace View
{
    internal class LootEspOverlay
    {
        internal static bool IsRunning = false;
        internal static string LootFilterText = "";
        internal static string RareFilterText = "";
        internal static string Input = "StrInput";
        internal static uint DockId;
        internal static bool IsRunning2 = false;
        internal static bool IsRunning3 = true;

        [ArmDot.Client.VirtualizeCode(Enable = false, Inherit = true)]
        internal static void Render()
        {
            if (ImGui.BeginTabBar("blah"))
            {
                if (ImGui.BeginTabItem("Настройки"))
                {
                    if (ImGui.Checkbox("Показывать лут", ref Program.Config.LootIsDraw))
                        Program.SaveData();

                    if (ImGui.SliderFloat("Дистанция отрисовки", ref Program.Config.LootDrawDistance, 1, 2000, "%.0f"))
                        Program.SaveData();

                    if (ImGui.SliderFloat("Минимальная цена (тыс.)", ref Program.Config.MinLootPrice_k, 0, 1000, "%.0f"))
                        Program.SaveData();

                    if (ImGui.Checkbox("Отображать предметы без цены", ref Program.Config.ShowItemsWithoutCost))
                        Program.SaveData();

                    if (ImGui.Checkbox("Отображать неизвестные предметы", ref Program.Config.ShowUnknownItems))
                        Program.SaveData();

                    if (ImGui.Checkbox("Отображать квестовые предметы", ref Program.Config.ShowQuestItems))
                        Program.SaveData();

                    if (ImGui.Checkbox("Отображать контейнеры", ref Program.Config.ShowContainers))
                        Program.SaveData();

                    if (ImGui.Checkbox("Лут в трупах(нагрузка fps)", ref Program.Config.LootInCorpse))
                        Program.SaveData();

                    ImGui.EndTabItem();
                }
                if (ImGui.BeginTabItem("Фильтр лута"))
                {
                    if (ImGui.InputTextMultiline("Фильтр лута", ref LootFilterText, 1000, new System.Numerics.Vector2(400, 600)))
                    {
                        Program.Config.LootFilter.Clear();
                        Program.Config.LootFilter = LootFilterText.Split('\n').ToList();
                        Program.SaveData();
                    }
                    ImGui.EndTabItem();
                }
                if (ImGui.BeginTabItem("Редкий лут"))
                {
                    if (ImGui.InputTextMultiline("Редкий лут", ref RareFilterText, 1000, new System.Numerics.Vector2(400, 600)))
                    {
                        Program.Config.RareLootFilter.Clear();
                        Program.Config.RareLootFilter = RareFilterText.Split('\n').Where(x => !string.IsNullOrEmpty(x)).ToList();
                        Program.SaveData();
                    }
                    ImGui.EndTabItem();
                }
                ImGui.EndTabBar();
            }
        }
    }
}
