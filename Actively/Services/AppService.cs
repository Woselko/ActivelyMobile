using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Actively.Models;
using System.Net;
using Actively.Utility.Cookies;

namespace Actively.Services
{
    public class AppService : IAppService
    {
        private readonly HttpClient _httpClient;

        public AppService(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("myServiceClient");
        }

        public async Task<Response> AuthenticateUser(LoginModel loginModel)
        {
            var returnResponse = new Response();
            var url = $"{Settings.BaseUrl}{Apis.AuthenticateUser}";
            var serializedStr = JsonConvert.SerializeObject(loginModel);
            var response = await _httpClient.PostAsync(url, new StringContent(serializedStr, Encoding.UTF8, "application/json"));
            string contentStr = await response.Content.ReadAsStringAsync();
            returnResponse = JsonConvert.DeserializeObject<Response>(contentStr);
            return returnResponse;
        }

        public async Task<List<string>> GetSupportedLanguages()
        {
            var languageList = new List<string>();
            var url = $"{Settings.BaseUrl}{Apis.GetSupportedCultures}";
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string contentStr = await response.Content.ReadAsStringAsync();
                languageList = JsonConvert.DeserializeObject <List<string>>(contentStr);
            }
            return languageList;
        }

		public async Task<Response> ChangeLanguage(string language)
		{
			var returnResponse = new Response();
			CookieContainer cookies = CookieContainerFactory.LoadCookiesFromSecureStorageToContainer();
			HttpClientHandler handler = new HttpClientHandler();
			handler.UseCookies = true;
			handler.CookieContainer = cookies;
			bool succes = false;
			using (var client = new HttpClient(handler))
			{
				var url = $"{Settings.BaseUrl}{Apis.ChangeLanguage}";

				var serializedStr = JsonConvert.SerializeObject(language);
                var response = await client.PostAsync(url, new StringContent(serializedStr, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
					succes = true;
					string FIRSTcontentStr = await response.Content.ReadAsStringAsync();
					returnResponse = JsonConvert.DeserializeObject<Response>(FIRSTcontentStr);
				}
            }

			if(succes)
			{
                Uri uri = new Uri(Settings.BaseUrl);
                IEnumerable<Cookie> responseCookies = cookies.GetCookies(uri).Cast<Cookie>();
                foreach (Cookie cookie in responseCookies)
                    cookies.Add(uri, cookie);
				await CookieContainerFactory.SaveCookiesToSecureStorage(cookies);
            }
            return returnResponse;
        }

		//Akcja wymagajaca autoryzacji token w Headerze requestu
        //public async Task<List<StudentModel>> GetAllStudents()
        //{
        //    var returnResponse = new List<StudentModel>();
        //    using (var client = new HttpClient())
        //    {
        //        var url = $"{Setting.BaseUrl}{Apis.GetAllStudents}";

        //        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {Setting.UserBasicDetail?.AccessToken}");
        //        var response = await client.GetAsync(url);

        //        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        //        {
        //            bool isTokenRefreshed = await RefreshToken();
        //            if (isTokenRefreshed) return await GetAllStudents();
        //        }
        //        else
        //        {
        //            if (response.IsSuccessStatusCode)
        //            {
        //                string contentStr = await response.Content.ReadAsStringAsync();
        //                var mainResponse = JsonConvert.DeserializeObject<Response>(contentStr);
        //                if (mainResponse.IsSuccess)
        //                {
        //                    returnResponse = JsonConvert.DeserializeObject<List<StudentModel>>(mainResponse.Content.ToString());
        //                }
        //            }
        //        }

        //    }
        //    return returnResponse;
        //}

        public async Task<bool> RefreshToken()
        {
            bool isTokenRefreshed = false;
            using (var client = new HttpClient())
            {
                var url = $"{Settings.BaseUrl}{Apis.RefreshToken}";

                var serializedStr = JsonConvert.SerializeObject(new AuthenticateRequestAndResponse
                {
                    RefreshToken = Settings.UserBasicDetail.RefreshToken,
                    AccessToken = Settings.UserBasicDetail.AccessToken
                });

                try
                {
                    var response = await client.PostAsync(url, new StringContent(serializedStr, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        string contentStr = await response.Content.ReadAsStringAsync();
                        var mainResponse = JsonConvert.DeserializeObject<Response>(contentStr);
                        if (mainResponse.IsSuccess)
                        {
                            var tokenDetails = JsonConvert.DeserializeObject<AuthenticateRequestAndResponse>(mainResponse.Content.ToString());
                            Settings.UserBasicDetail.AccessToken = tokenDetails.AccessToken;
                            Settings.UserBasicDetail.RefreshToken = tokenDetails.RefreshToken;

                            string userDetailsStr = JsonConvert.SerializeObject(Settings.UserBasicDetail);
                            await SecureStorage.SetAsync(nameof(Settings.UserBasicDetail), userDetailsStr);
                            isTokenRefreshed = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }
            return isTokenRefreshed;
        }

        public async Task<(bool IsSuccess, string Message)> RegisterUser(RegisterUser registerUser)
        {
            string message = string.Empty;
                var url = $"{Settings.BaseUrl}{Apis.RegisterUser}";

                var serializedStr = JsonConvert.SerializeObject(registerUser);
            var response = await _httpClient.PostAsync(url, new StringContent(serializedStr, Encoding.UTF8, "application/json"));
 
            string contentStr = await response.Content.ReadAsStringAsync();
            message = JsonConvert.DeserializeObject<Response>(contentStr).Message;
            return (response.IsSuccessStatusCode, message);
        }
    }
}