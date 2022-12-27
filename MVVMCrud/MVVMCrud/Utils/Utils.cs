using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MVVMCrud.Utils
{
    public static class Utils
    {
        public static bool IsConnected()
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.Internet)
            {
                // Connection to internet is available
                return true;
            }

            return false;
        }

        public static void DisplaySimplyAlert(string title, string message, string ok = null)
        {
            if (string.IsNullOrEmpty(ok))
            {
                ok = MVVMCrudApplication.GetOKText();
            }
            Application.Current.MainPage.DisplayAlert(title, message, ok);
        }

        public static MultipartFormDataContent GetFormDataToUpload(object item, JsonSerializerSettings settings)
        {
            var content = JsonConvert.SerializeObject(item, settings);

            var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(content);

            var formData = new MultipartFormDataContent();
            foreach (var keyValue in values)
            {
                var key = keyValue.Key;
                var value = keyValue.Value;

                if (value != null)
                {
                    formData.Add(new StringContent(value.ToString(), Encoding.UTF8, "application/json"), key);
                }
            }

            return formData;
        }
    }
}
