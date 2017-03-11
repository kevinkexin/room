using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNERoomPlus;

namespace UNERoomPlusBackEnd
{
    public class SystemValue
    {
        public static string WeatherAPIKey { get; set; }
        public static string DefaultCityTag { get; set; }
        public static string[] WeatherLanguage { get; set; }

        public static string GetConfig(string Key)
        {
            try
            {
                
                string strValue = System.Configuration.ConfigurationManager.AppSettings[Key];

                if (string.IsNullOrEmpty(strValue))
                    return string.Empty;
                else
                    return strValue;
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError(ex);
                return string.Empty;
                //throw ex;
            }
        }

        public static void LoadConfigure()
        {
            try
            {
                //encryptor = new KEXIN.EncryptionUtility();

                //string TerminalId = cn.Singsoft.FingerPrint.Value().ToUpper();

                //encryptor.passPhrase = TerminalId.Substring(0, 1) +
                //                       TerminalId.Substring(9, 1) +
                //                       TerminalId.Substring(14, 1) +
                //                       TerminalId.Substring(19, 1) +
                //                       TerminalId.Substring(24, 1);

                //PublicURL = encryptor.Decrypt(GetConfig("PublicURL"));
                //ClientId = encryptor.Decrypt(GetConfig("ClientId"));
                //SecrectKey = encryptor.Decrypt(GetConfig("SecrectKey"));

                //if (!PublicURL.EndsWith("/"))
                //    PublicURL += "/";

                WeatherAPIKey = GetConfig("WeatherAPIKey");
                DefaultCityTag = GetConfig("DefaultCityTag");
                WeatherLanguage = GetConfig("WeatherLanguageCode").Split(',');
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError(ex);
            }
        }
    }
}
