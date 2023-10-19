using ImGuiNET;
using System.Numerics;

namespace ClickableTransparentOverlay
{
    internal class ImElements
    {
        internal static bool Link(string label, string description, Vector4 color)
        {
            var size = ImGui.CalcTextSize(label);
            var p = ImGui.GetCursorScreenPos();
            var p2 = ImGui.GetCursorPos();
            var result = ImGui.InvisibleButton(label, size);

            ImGui.SetCursorPos(p2);

            if (ImGui.IsItemHovered())
            {
                if (!string.IsNullOrEmpty(description))
                {
                    ImGui.BeginTooltip();
                    ImGui.PushTextWrapPos(600);
                    ImGui.TextUnformatted(description);
                    ImGui.PopTextWrapPos();
                    ImGui.EndTooltip();

                }

                ImGui.TextColored(color, label);
                ImGui.GetWindowDrawList().AddLine(new Vector2(p.X, p.Y + size.Y), new Vector2(p.X + size.X, p.Y + size.Y), ImGui.GetColorU32(color));
            }
            else
            {
                ImGui.TextColored(color, label);
            }

            return result;
        }
    }
}
