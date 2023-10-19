namespace ClickableTransparentOverlay.Win32
{
    using System;
    using System.Runtime.InteropServices;

    internal static class Kernel32
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetModuleHandle(string? lpModuleName);
    }
}
