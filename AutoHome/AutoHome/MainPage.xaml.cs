using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AutoHome
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        ObservableCollection<LightSwitch> observableSwitches = new ObservableCollection<LightSwitch>();

        public MainPage()
        {
            this.InitializeComponent();

            SwitchesCVS.Source = observableSwitches;
        }

        private async Task<AvailableSwitchesResponse> getAvailableSwitchesAsync()
        {
            string UrlRequest = "http://192.168.1.4:8080/json.htm?type=command&param=getlightswitches";

            try
            {
                HttpWebRequest request = WebRequest.Create(UrlRequest) as HttpWebRequest;

                using (HttpWebResponse response = (await request.GetResponseAsync()) as HttpWebResponse)
                {
                    if(response.StatusCode != HttpStatusCode.OK)
                    {
                        throw new Exception(String.Format("Server error (HTTP {0}: {1}).",
                            response.StatusCode,
                            response.StatusDescription));
                    }
                    DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(AvailableSwitchesResponse));
                    object objResponse = jsonSerializer.ReadObject(response.GetResponseStream());
                    AvailableSwitchesResponse jsonResponse = objResponse as AvailableSwitchesResponse;
                    return jsonResponse;
                }
            }
            catch (Exception e)
            {
                throw;
            }

        }

        private async void Page_Loading(FrameworkElement sender, object args)
        {

            //ContactsCVS.Source = Contact.GetContactsGrouped(250);
            AvailableSwitchesResponse availableSwitches = await getAvailableSwitchesAsync();
            
            foreach (LightSwitch s in availableSwitches.LightSwitches)
            {
                observableSwitches.Add(s);
            }
        }

        private async Task setSwitchAsync(LightSwitch switchToSwitch, Boolean value)
        {
            string switchcmd = value == true ? "On" : "False";
            string UrlRequest = string.Format("http://192.168.1.4:8080/json.htm?type=command&param=switchlight&idx={0}&switchcmd={1}",switchToSwitch.idx, switchcmd);

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
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private async void btnGo_Click(object sender, RoutedEventArgs e)
        {
            if(lstSwitches.SelectedItems.Count > 0)
            {
                LightSwitch selectedSwitch = lstSwitches.SelectedItem as LightSwitch;
                await setSwitchAsync(selectedSwitch, true);

            }

        }

        private void ShowHamburgerMenu(object sender, RoutedEventArgs e)
        {
            HamburgerMenu.HamburgerSplitView.IsPaneOpen = !HamburgerMenu.HamburgerSplitView.IsPaneOpen;
        }
    }

    [DataContract]
    public class AvailableSwitchesResponse
    {
        [DataMember(Name = "result")]
        public LightSwitch[] LightSwitches { get; set; }

        [DataMember(Name = "status")]
        public string Status { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }
    }

    [DataContract]
    public class LightSwitch
    {
        [DataMember(Name = "IsDimmer")]
        public Boolean IsDimmer { get; set; }

        [DataMember(Name = "Name")]
        public string Name { get; set; }

        [DataMember(Name = "SubType")]
        public string SubType { get; set; }

        [DataMember(Name ="Type")]
        public string Type { get; set; }

        [DataMember(Name = "idx")]
        public int idx { get; set; }
    }
}
