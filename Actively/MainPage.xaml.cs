namespace Actively
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
		}

		protected override void OnHandlerChanged()
		{
			//#if WINDOWS
			//			            var webview = blazorWebView.Handler.PlatformView as Microsoft.UI.Xaml.Controls.WebView2;
			//			            var cookie = webview.CoreWebView2.CookieManager.CreateCookie();
			//			            webview.CoreWebView2.CookieManager.AddOrUpdateCookie();
			//						webview.CoreWebView2.CookieManager.GetCookiesAsync();
			//#endif
#if ANDROID

			var webview = blazorWebView.Handler.PlatformView as Android.Webkit.WebView;
			Android.Webkit.CookieManager.Instance.SetAcceptCookie(true);
			Android.Webkit.CookieManager.Instance.SetAcceptThirdPartyCookies(webview, true);
			//			webview.Settings.JavaScriptEnabled = true;
			//          webview.Settings.DomStorageEnabled = true;

			#endif
			//			base.OnHandlerChanged();
		}

	}
}