using System;
using System.Collections.Generic;
using System.Text;
using XLabs.Data;
using Humanizer;
using System.Net.Http;
using System.Threading.Tasks;
using Plugin.DeviceInfo;

namespace TutorApp2.Models
{
    public static class TwilioHelper
    {
        public static string Identity { get; private set; }

        public static async Task<string> GetTokenAsync()
        {
            var id = CrossDeviceInfo.Current.Id;

            var tokenEndpoint = $"https://xamarinchat.azurewebsites.net/token?device={id}";

            var http = new HttpClient();
            var data = await http.GetStringAsync(tokenEndpoint);

            var response = Newtonsoft.Json.JsonConvert.DeserializeObject<TwilioResponse>(data);

            Identity = response.Identity?.Trim('"') ?? string.Empty;

            return response?.Token?.Trim('"') ?? string.Empty;
        }
    }

    public class TwilioResponse
    {
        [Newtonsoft.Json.JsonProperty("identity")]
        public string Identity { get; set; } = string.Empty;

        [Newtonsoft.Json.JsonProperty("token")]
        public string Token { get; set; } = string.Empty;
    }

    public interface ITwilioMessenger
    {
        Task<bool> InitializeAsync();

        void SendMessage(string text);

        Action<Message> MessageAdded { get; set; }
    }
    public class Message : ObservableObject
    {
        string text;

        public string Text
        {
            get { return text; }
            set { SetProperty(ref text, value); }
        }

        DateTime messageDateTime;

        public DateTime MessageDateTime
        {
            get { return messageDateTime; }
            set { SetProperty(ref messageDateTime, value); }
        }

        public string MessageTimeDisplay => MessageDateTime.Humanize();

        bool isIncoming;

        public bool IsIncoming
        {
            get { return isIncoming; }
            set { SetProperty(ref isIncoming, value); }
        }

        public bool HasAttachement => !string.IsNullOrEmpty(attachementUrl);

        string attachementUrl;

        public string AttachementUrl
        {
            get { return attachementUrl; }
            set { SetProperty(ref attachementUrl, value); }
        }

    }
}
