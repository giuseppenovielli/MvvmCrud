using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MVVMCrud.Utils
{
    public static partial class Utils
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

        public static FormUrlEncodedContent GetFormUrlEncodedContent(object item, JsonSerializerSettings settings)
        {
            var content = JsonConvert.SerializeObject(item, settings);

            var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(content);

            var values = new List<KeyValuePair<string, string>>();

            foreach (var keyValue in dict)
            {
                var key = keyValue.Key;
                var value = keyValue.Value.ToString();

                if (value != null)
                {
                    values.Add(new KeyValuePair<string, string>(key, value));
                }
            }

            return new FormUrlEncodedContent(values);
        }

        public static string[] SplitOnUppercase(string item)
        {
            //https://www.codegrepper.com/code-examples/csharp/c%23+split+string+by+caps
            return Regex.Split(item, @"(?<!^)(?=[A-Z])");
        }

        public static string GetPageNameWithUnderscore(string className, string contextName)
        {
            var pageWithUnderscore = string.Empty;

            var pageContext = className.Split(new[] { contextName }, StringSplitOptions.None);
            if (pageContext?.Length == 2)
            {
                var pageName = pageContext[0];
                var list = SplitOnUppercase(pageName);

                var listLenght = list.Length;
                for (int i = 0; i < list.Length; i++)
                {
                    pageWithUnderscore += list[i];
                    if (i < listLenght - 1)
                    {
                        pageWithUnderscore += "_";
                    }
                }
            }

            return pageWithUnderscore.ToLower();
        }
    }
}
