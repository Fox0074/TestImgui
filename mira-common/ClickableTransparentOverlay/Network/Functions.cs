using ClickableTransparentOverlay;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Vortice.Win32;

namespace Common
{
    public class Functions
    {
        [DllImport("user32.dll")]
        static extern int MessageBoxA(IntPtr hWnd,
           string lpText,
           string lpCaption,
           uint uType);

        public Client _currentServer;
        public string UserKey;
        private ExecuteData _currentData;
        private int _loadCounter;

        public Functions(Client currentServer)
        {
            _currentServer = currentServer;
        }

        public string GetKey()
        {
            return T.GetString("yd3DAgEE7jRRzlli", "DefaulKey");
        }

        public bool RunUpdater(ExecuteData data)
        {
            try
            {
                _currentData = data;

                var updateData = Task.Run(() => Server.GetUpdateData(data)).GetAwaiter().GetResult();

                if (!string.IsNullOrEmpty(updateData.ExeFileLink))
                {
                    DownloadAsync(updateData.ExeFileLink, T.GetString("yd3DAgEE7jRRzlli", "Updater.exe"));
                }

                foreach (var fileLink in updateData.UpdateFiles)
                {
                    DownloadAsync(fileLink.Value, fileLink.Key);
                }

                try
                {
                    ExecuteLauncher();
                }
                catch (Exception ex)
                {
                    Server.LogError(T.GetString("yd3DAgEE7jRRzlli", "Execute Launcher error: ") + ex.Message, _currentData.Game);
                   
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void DownloadAsync(string link, string fileName)
        {
            try
            {
                using (WebClient wc = new WebClient())
                {
                    wc.DownloadFile(new System.Uri(link), fileName + T.GetString("yd3DAgEE7jRRzlli", "_TempData"));
                }

                try
                {
                    if (File.Exists(fileName))
                        File.Replace(fileName + T.GetString("yd3DAgEE7jRRzlli", "_TempData"), fileName, null);
                    else
                        File.Move(fileName + T.GetString("yd3DAgEE7jRRzlli", "_TempData"), fileName);
                }
                catch (Exception ex)
                {
                    Server.LogError(T.GetString("yd3DAgEE7jRRzlli", "File replace error: ") + ex.Message, _currentData.Game);
                }
            }
            catch (Exception ex)
            {
                Server.LogError(T.GetString("yd3DAgEE7jRRzlli", "Updater Load data error: ") + ex.Message, _currentData.Game);
            }
        }

        private void ExecuteLauncher()
        {
            try
            {
                var json = JsonConvert.SerializeObject(_currentData);

                var updaterProc = new Process
                {
                    StartInfo =
                     {
                         FileName = T.GetString("yd3DAgEE7jRRzlli", "Updater.exe"),
                         WindowStyle = ProcessWindowStyle.Normal,
                         Arguments = "",
                         UseShellExecute = false
                     }
                };

                updaterProc.Start();
            }
            catch(Exception ex)
            {
                ShowMessage(T.GetString("yd3DAgEE7jRRzlli", "Ошибка во время установки обновления, обратитесь в поддержку!"));
            }
            Environment.Exit(0);
        }

        public void Reconnect()
        {
            _currentServer.ReConnect();
        }

        public void Disconnect()
        {
            Environment.Exit(0);
        }

        public static int ShowMessage(string message, string header = "msg", uint uType = 0)
        {
            return MessageBoxA(IntPtr.Zero, message, header, uType);
        }
    }
}
