using Actively.Data;
using Actively.Models;
using Actively.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace Actively
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
		    builder.Services.AddBlazorWebViewDeveloperTools();
		    builder.Logging.AddDebug();
#endif
			Settings.BaseUrl = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:6010" : "http://localhost:6010";

			builder.Services.AddBlazorWebView();

			builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

			builder.Services.Configure<RequestLocalizationOptions>(options =>
			{
				var supportedCultures = new[]
				{
					new CultureInfo("en"),
					new CultureInfo("pl"),
				};
				options.DefaultRequestCulture = new RequestCulture("en");
				options.SupportedCultures = supportedCultures;
				options.SupportedUICultures = supportedCultures;
			});

			//builder.Services.AddHttpClient<DishClient>(client => { client.BaseAddress = new Uri(APIUrl); });

			builder.Services.AddSingleton<IAppService, AppService>();
            builder.Services.AddSingleton<WeatherForecastService>();

            return builder.Build();
        }
    }
}