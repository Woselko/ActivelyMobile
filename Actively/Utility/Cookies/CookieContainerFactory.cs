using Actively.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text;
using System.Threading.Tasks;

namespace Actively.Utility.Cookies
{
	public static class CookieContainerFactory
	{
		public static async Task SaveCookiesToSecureStorage(CookieContainer cookieContainer)
		{
			var cookieSerializedString = GetCookies(cookieContainer);
			Settings.LanguageCookie = cookieSerializedString;
            await SecureStorage.SetAsync(nameof(Settings.LanguageCookie), cookieSerializedString);
        }

		public static CookieContainer LoadCookiesFromSecureStorageToContainer()
		{
			var languageCookie = Settings.LanguageCookie;
			return CookieContainerFactor(languageCookie);
		}

		private static string GetCookies(CookieContainer cookieContainer)
		{
			using (MemoryStream stream = new MemoryStream())
			{
                //new BinaryFormatter().Serialize(stream, cookieContainer);
                SoapFormatter converter = new SoapFormatter();
                converter.Serialize(stream, cookieContainer);
                var bytes = new byte[stream.Length];
				stream.Position = 0;
				stream.Read(bytes, 0, bytes.Length);
				//return Convert.ToBase64String(bytes);
				return Encoding.UTF8.GetString(bytes);

            }
		}

        static CookieContainer CookieContainerFactor(string cookieText)
        {
            try
            {
                var bytes = Encoding.UTF8.GetBytes(cookieText);
                //var bytes = Convert.FromBase64String(cookieText);
                using (MemoryStream stream = new MemoryStream(bytes))
                {
                    //return (CookieContainer)new BinaryFormatter().Deserialize(stream);
                    SoapFormatter converter = new SoapFormatter();
                    var container = converter.Deserialize(stream) as CookieContainer;
                    return container;
                }
            }
            catch
            {
                return new CookieContainer();
            }
        }

        private static void SetCookies(CookieContainer cookieContainer, string cookieText)
		{
			try
			{
				var bytes = Convert.FromBase64String(cookieText);
				using (MemoryStream stream = new MemoryStream(bytes))
				{
                    //cookieContainer = (CookieContainer)new BinaryFormatter().Deserialize(stream);
                    SoapFormatter converter = new SoapFormatter();
                    cookieContainer = converter.Deserialize(stream) as CookieContainer;
                }
			}
			catch
			{
				//Ignore if the string is not valid.
			}
		}

        //Maybe will be usefull somedays..
        #region UnusedCode
        //public static void WriteCookiesToDisk(string file, CookieContainer cookieJar)
        //{
        //    using (Stream stream = File.Create(file))
        //    {
        //        try
        //        {
        //            Console.Out.Write("Writing cookies to disk... ");
        //            BinaryFormatter formatter = new BinaryFormatter();
        //            formatter.Serialize(stream, cookieJar);
        //            Console.Out.WriteLine("Done.");
        //        }
        //        catch (Exception e)
        //        {
        //            Console.Out.WriteLine("Problem writing cookies to disk: " + e.GetType());
        //        }
        //    }
        //}

        //public static CookieContainer ReadCookiesFromDisk(string file)
        //{

        //    try
        //    {
        //        using (Stream stream = File.Open(file, FileMode.Open))
        //        {
        //            Console.Out.Write("Reading cookies from disk... ");
        //            BinaryFormatter formatter = new BinaryFormatter();
        //            Console.Out.WriteLine("Done.");
        //            return (CookieContainer)formatter.Deserialize(stream);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.Out.WriteLine("Problem reading cookies from disk: " + e.GetType());
        //        return new CookieContainer();
        //    }
        //}

        //public static void LoadCookiesFromFile(string path, CookieContainer cookieContainer)
        //{
        //    SetCookies(cookieContainer, File.ReadAllText(path));
        //}

        //public static void SaveCookiesToFile(string path, CookieContainer cookieContainer)
        //{
        //    File.WriteAllText(path, GetCookies(cookieContainer));
        //}

        //public static CookieContainer LoadCookiesFromFileToContainer(string path)
        //{
        //    return CookieContainerFactor(File.ReadAllText(path));
        //}
        #endregion
    }
}
