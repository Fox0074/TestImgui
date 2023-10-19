using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ClickableTransparentOverlay.Styles
{
    public class OrangeStyle : BaseStyle
    {
        public OrangeStyle() 
        {
            MainColor = new Vector4(0.76f, 0.31f, 0, 1);
            MainActiveColor = new Vector4(0.86f, 0.41f, 0f, 1);
            MainUnActiveColor = new Vector4(0.10f, 0.09f, 0.12f, 1);
            MainHoveredColor = new Vector4(0.86f, 0.41f, 0f, 1);
        }

        public override void InitStyle()
        {
            var style = ImGui.GetStyle();
            var colors = style.Colors;

            style.WindowPadding = new Vector2(15, 15);
            style.WindowRounding = 6f;
            style.FramePadding = new Vector2(5, 5);
            style.FrameRounding = 4f;
            style.ItemSpacing = new Vector2(12, 8);
            style.ItemInnerSpacing = new Vector2(8, 6);
            style.IndentSpacing = 25f;
            style.ScrollbarSize = 15f;
            style.ScrollbarRounding = 9f;
            style.GrabMinSize = 5f;
            style.GrabRounding = 3f;

            colors[(int)ImGuiCol.Text] = new Vector4(0.80f, 0.80f, 0.83f, 1f);
            colors[(int)ImGuiCol.TextDisabled] = new Vector4(0.24f, 0.23f, 0.29f, 1f);
            colors[(int)ImGuiCol.WindowBg] = new Vector4(0.06f, 0.05f, 0.07f, 1f);
            colors[(int)ImGuiCol.ChildBg] = new Vector4(0.07f, 0.07f, 0.09f, 1f);
            colors[(int)ImGuiCol.PopupBg] = new Vector4(0.07f, 0.07f, 0.09f, 1f);
            colors[(int)ImGuiCol.Border] = new Vector4(0.80f, 0.80f, 0.83f, 0.88f);
            colors[(int)ImGuiCol.BorderShadow] = new Vector4(0.92f, 0.91f, 0.88f, 0f);
            colors[(int)ImGuiCol.FrameBg] = new Vector4(0.10f, 0.09f, 0.12f, 1f);
            colors[(int)ImGuiCol.FrameBgHovered] = new Vector4(0.24f, 0.23f, 0.29f, 1);
            colors[(int)ImGuiCol.FrameBgActive] = new Vector4(0.56f, 0.56f, 0.58f, 1);
            colors[(int)ImGuiCol.TitleBg] = new Vector4(0.76f, 0.31f, 0, 1);
            colors[(int)ImGuiCol.TitleBgCollapsed] = new Vector4(1f, 0.98f, 0.95f, 0.75f);
            colors[(int)ImGuiCol.TitleBgActive] = new Vector4(0.80f, 0.33f, 0, 1);
            colors[(int)ImGuiCol.MenuBarBg] = new Vector4(0.10f, 0.09f, 0.12f, 1);
            colors[(int)ImGuiCol.ScrollbarBg] = new Vector4(0.10f, 0.09f, 0.12f, 1);
            colors[(int)ImGuiCol.ScrollbarGrab] = new Vector4(0.80f, 0.80f, 0.83f, 0.31f);
            colors[(int)ImGuiCol.ScrollbarGrabHovered] = new Vector4(0.56f, 0.56f, 0.58f, 1);
            colors[(int)ImGuiCol.ScrollbarGrabActive] = new Vector4(0.06f, 0.05f, 0.07f, 1);
            colors[(int)ImGuiCol.CheckMark] = new Vector4(1, 0.42f, 0, 0.53f);
            colors[(int)ImGuiCol.SliderGrab] = new Vector4(1, 0.42f, 0, 0.53f);
            colors[(int)ImGuiCol.SliderGrabActive] = new Vector4(1, 0.42f, 0, 1);
            colors[(int)ImGuiCol.Button] = new Vector4(0.10f, 0.09f, 0.12f, 1);
            colors[(int)ImGuiCol.ButtonHovered] = new Vector4(0.24f, 0.23f, 0.29f, 1);
            colors[(int)ImGuiCol.ButtonActive] = new Vector4(0.56f, 0.56f, 0.58f, 1);
            colors[(int)ImGuiCol.Header] = new Vector4(0.10f, 0.09f, 0.12f, 1);
            colors[(int)ImGuiCol.HeaderHovered] = new Vector4(0.56f, 0.56f, 0.58f, 1);
            colors[(int)ImGuiCol.HeaderActive] = new Vector4(0.06f, 0.05f, 0.07f, 1);
            colors[(int)ImGuiCol.ResizeGrip] = new Vector4(0, 0, 0, 0);
            colors[(int)ImGuiCol.ResizeGripHovered] = new Vector4(0.56f, 0.56f, 0.58f, 1);
            colors[(int)ImGuiCol.ResizeGripActive] = new Vector4(0.06f, 0.05f, 0.07f, 1);

            colors[(int)ImGuiCol.TabActive] = new Vector4(0.76f, 0.31f, 0, 1);
            colors[(int)ImGuiCol.TabHovered] = new Vector4(0.86f, 0.41f, .1f, 1);
            colors[(int)ImGuiCol.Tab] = new Vector4(0.10f, 0.09f, 0.12f, 1);

            colors[(int)ImGuiCol.PlotLines] = new Vector4(0.40f, 0.39f, 0.38f, 0.63f);
            colors[(int)ImGuiCol.PlotLinesHovered] = new Vector4(0.25f, 1, 0, 1);
            colors[(int)ImGuiCol.PlotHistogram] = new Vector4(0.40f, 0.39f, 0.38f, 0.63f);
            colors[(int)ImGuiCol.PlotHistogramHovered] = new Vector4(0.25f, 1, 0, 1);
            colors[(int)ImGuiCol.TextSelectedBg] = new Vector4(0.25f, 1, 0, 0.43f);
            colors[(int)ImGuiCol.ModalWindowDimBg] = new Vector4(1, 0.98f, 0.95f, 0.73f);
        }
    }
}
