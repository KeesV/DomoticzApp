using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AutHome.Domoticz.Api
{
    public class DomoticzConnection
    {
        public static DomoticzConnection Instance { get; }

        public static string DomoticzServer { get; }
        public static int DomoticzPort { get; }

        static DomoticzConnection()
        {
            // implement singleton pattern
            Instance = Instance ?? new DomoticzConnection();
        }

        private async Task TestDomoticzConnection()
        {
            string UrlRequest = $"http://{DomoticzServer}:{DomoticzPort}/json.htm?type=command&param=addlogmessage&message=HelloFromWindows10App";

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
    }
}
