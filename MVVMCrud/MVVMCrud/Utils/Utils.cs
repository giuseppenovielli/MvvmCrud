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
    }
}
