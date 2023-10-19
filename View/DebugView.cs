using ClickableTransparentOverlay;
using ImGuiNET;
using MiraEFK;
using System.Numerics;

namespace UserInterfaceEFK.View
{
    internal class DebugView
    {
        private static List<float> _time = new List<float>();
        [ArmDot.Client.VirtualizeCode(Enable = false, Inherit = true)]
        public static void Render()
        {
            ImGui.Begin("Debug", ref Program.Config.ShowDebug,
               ImGuiWindowFlags.NoInputs |
               ImGuiWindowFlags.NoCollapse |
               ImGuiWindowFlags.NoTitleBar |
               ImGuiWindowFlags.AlwaysAutoResize |
               ImGuiWindowFlags.NoResize);

            ImGui.SetWindowPos(new Vector2(5, 5));
            _time.Add(ImGui.GetIO().DeltaTime);
            while (_time.Sum() > 1)
            {
                _time.RemoveAt(0);
            }

            ImGui.Text("FPS: " + ImGui.GetIO().Framerate.ToString("0."));
            ImGui.Text("delta Time: " + ImGui.GetIO().DeltaTime);
            ImGui.Text("Loop Fps: " + _time.Count);

            ImGui.End();
        }
    }
}
