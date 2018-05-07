using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using Windows.Web.UI.Interop;

namespace WebAuthenticationBroker.Desktop
{
    public class WebAuthenticationBroker
    {
        public static async Task<WebAuthenticationResult> AuthenticateAsync(
            Uri requestUri, Uri callbackUri, Window parentWindow)
        {
            var webViewControlProcess = new WebViewControlProcess();
            WebViewControl webViewControl = null;

            var webViewWindow = new WebAuthenticationWindow();
            webViewWindow.Owner = parentWindow;

            // Response details
            string responseData = "";
            uint responseCode = 0;
            var status = WebAuthenticationStatus.Success;

            webViewWindow.Loaded += async (_, __) =>
            {
                await Task.Delay(200);

                webViewControl = await webViewControlProcess.CreateWebViewControlAsync(
                    new WindowInteropHelper(webViewWindow).Handle.ToInt64(),
                    new Windows.Foundation.Rect(0, 0, webViewWindow.ActualWidth, webViewWindow.ActualHeight));

                webViewControl.NavigationStarting += (s, e) =>
                {
                    // Check for the Uri first -- localhost will give a 404
                    // but we might already have the data we want
                    if (e.Uri.ToString().StartsWith(callbackUri.ToString()))
                    {
                        responseData = e.Uri.ToString();
                        webViewWindow.DialogResult = true;
                    }
                };

                webViewControl.NavigationCompleted += (s, e) =>
                {
                    if (!e.IsSuccess)
                    {
                        webViewWindow.DialogResult = false;
                        responseCode = (uint)e.WebErrorStatus;
                    }
                };

                webViewControl.Navigate(requestUri);
            };

            var dialogResult = await ShowDialogAsync(webViewWindow);
            if (dialogResult.HasValue)
            {
                status = dialogResult.Value ? WebAuthenticationStatus.Success : WebAuthenticationStatus.Error;
            }
            else
            {
                status = WebAuthenticationStatus.Canceled;
            }
            
            webViewControlProcess.Terminate();

            return new WebAuthenticationResult(responseData, responseCode, status);
        }

        public static Task<bool?> ShowDialogAsync(Window self)
        {
            if (self == null) throw new ArgumentNullException("self");

            TaskCompletionSource<bool?> completion = new TaskCompletionSource<bool?>();
            self.Dispatcher.BeginInvoke(new Action(() => completion.SetResult(self.ShowDialog())));

            return completion.Task;
        }
    }
}
