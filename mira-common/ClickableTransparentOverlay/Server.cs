using ClickableTransparentOverlay;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class Server
    {
        public static bool IsSendError;
        private static HttpClient httpClient = new HttpClient()
        {
            BaseAddress = new Uri("https://mira-hack.ru/api/"),
        };

        public static async Task<UpdateData> GetUpdateData(ExecuteData data)
        {
            var info = await PostRequest<UpdateResponce>(new UpdateRequest() 
            {
                Method = T.GetString("yd3DAgEE7jRRzlli", "GetUpdateData"), 
                Game = T.GetString("yd3DAgEE7jRRzlli", "Updater"),
                ExeName = data.ExeName,
                Token = data.Token,
                UseExperementalVersion = data.UseExperementalVersion,
                UpdaterVersion = data.UpdaterVersion
            });

            return info.Data;
        }

        public static async Task<DateTime> GetKeyEndTime(string token)
        {
            var info = await PostRequest<StringDataResponce>(new BaseRequest()
            {
                Method = T.GetString("yd3DAgEE7jRRzlli", "GetKeyExitTime"),
                Game = T.GetString("yd3DAgEE7jRRzlli", "Test"),
                Token = token
            });

            return DateTime.Parse(info.Data);
        }

        public static async void LogError(string message, string game)
        {
            IsSendError = true;
            await PostRequest<BaseResponce>(new LogErrorRequest() { Method = T.GetString("yd3DAgEE7jRRzlli", "ErrorLog"), Game = game, Message = message });
            IsSendError = false;
        }

        private static async Task<T> PostRequest<T>(BaseRequest requestData) where T : BaseResponce
        {
            try
            {
                var json = JsonConvert.SerializeObject(requestData);
                byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
                var content = new StringContent(json, Encoding.UTF8, ClickableTransparentOverlay.T.GetString("yd3DAgEE7jRRzlli", "application/json"));

                HttpResponseMessage response = await httpClient.PostAsync(httpClient.BaseAddress, content);

                if (!response.IsSuccessStatusCode)
                {
                    LogError(string.Format(ClickableTransparentOverlay.T.GetString("yd3DAgEE7jRRzlli", "Server request {0} failed code: {1}"), requestData.Method, response.StatusCode), requestData.Game);
                    return null;
                }

                try
                {
                    var result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(result);
                }
                catch (Exception e)
                {
                    LogError(ClickableTransparentOverlay.T.GetString("yd3DAgEE7jRRzlli", "Unable to parse server response: ") + e.Message, requestData.Game);
                    return null;
                }
            }
            catch (Exception ex)
            {
                if (!IsSendError)
                {
                    LogError(ClickableTransparentOverlay.T.GetString("yd3DAgEE7jRRzlli", "Error: ") + ex.Message, requestData.Game);
                }
                return null;
            }

        }
    }
}
