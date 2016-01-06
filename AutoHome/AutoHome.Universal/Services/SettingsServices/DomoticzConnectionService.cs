using AutoHome.Universal.Services.SettingsServices;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace AutoHome.Universal.Services.DomoticzConnectionServices
{
    public class DomoticzConnection
    {
        public static DomoticzConnection Instance { get; }

        SettingsService _settings;

        static DomoticzConnection()
        {
            // implement singleton pattern
            Instance = Instance ?? new DomoticzConnection();
        }

        private DomoticzConnection()
        {
            _settings = SettingsService.Instance;
        }

        public async Task TestDomoticzConnectionOld()
        {
            string UrlRequest = $"http://{_settings.DomoticzServer}:{_settings.DomoticzPort}/json.htm?type=command&param=addlogmessage&message=HelloFromWindows10App";

            try
            {
                HttpWebRequest request = WebRequest.Create(UrlRequest) as HttpWebRequest;

                using (HttpWebResponse response = (await request.GetResponseAsync()) as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        throw new Exception(String.Format("Server error (HTTP {0}: {1}).",
                            response.StatusCode,
                            response.StatusDescription));
                    } 
                    //DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(AvailableSwitchesResponse));
                    //object objResponse = jsonSerializer.ReadObject(response.GetResponseStream());
                    //AvailableSwitchesResponse jsonResponse = objResponse as AvailableSwitchesResponse;
                    //return jsonResponse;
                }
            }
            catch (Exception e)
            {
                throw;
            }

        }

        public async Task TestDomoticzConnection()
        {
            using (var client = new HttpClient())
            {

            }
        }
    }
}
