using System;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace AutoHome.Universal.ViewModels
{
    public class SettingsPageViewModel : AutoHome.Universal.Mvvm.ViewModelBase
    {
        public ConnectionPartViewModel ConnectionPartViewModel { get; } = new ConnectionPartViewModel();
        public SettingsPartViewModel SettingsPartViewModel { get; } = new SettingsPartViewModel();
        public AboutPartViewModel AboutPartViewModel { get; } = new AboutPartViewModel();
    }

    public class ConnectionPartViewModel : Mvvm.ViewModelBase
    {
        Services.SettingsServices.SettingsService _settings;

        public ConnectionPartViewModel()
        {
            if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                _settings = Services.SettingsServices.SettingsService.Instance;
        }

        public string DomoticzServer {
            get { return _settings.DomoticzServer; }
            set { _settings.DomoticzServer = value;  base.RaisePropertyChanged(); }
        }

        public int DomoticzPort
        {
            get { return _settings.DomoticzPort; }
            set { _settings.DomoticzPort = value; base.RaisePropertyChanged(); }
        }

        public async Task TestConnection()
        {
            await TestDomoticzConnection();
        }

        private async Task TestDomoticzConnection()
        {
            string UrlRequest = "http://192.168.1.4:8080/json.htm?type=command&param=addlogmessage&message=HelloFromWindows10App";

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

    public class SettingsPartViewModel : Mvvm.ViewModelBase
    {
        Services.SettingsServices.SettingsService _settings;

        public SettingsPartViewModel()
        {
            if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                _settings = Services.SettingsServices.SettingsService.Instance;
        }

        public bool UseShellBackButton
        {
            get { return _settings.UseShellBackButton; }
            set { _settings.UseShellBackButton = value; base.RaisePropertyChanged(); }
        }

        public bool UseLightThemeButton
        {
            get { return _settings.AppTheme.Equals(ApplicationTheme.Light); }
            set { _settings.AppTheme = value ? ApplicationTheme.Light : ApplicationTheme.Dark; base.RaisePropertyChanged(); }
        }

        private string _BusyText = "Please wait...";
        public string BusyText
        {
            get { return _BusyText; }
            set { Set(ref _BusyText, value); }
        }

        public void ShowBusy()
        {
            Views.Shell.SetBusy(true, _BusyText);
        }

        public void HideBusy()
        {
            Views.Shell.SetBusy(false);
        }
    }

    public class AboutPartViewModel : Mvvm.ViewModelBase
    {
        public Uri Logo => Windows.ApplicationModel.Package.Current.Logo;

        public string DisplayName => Windows.ApplicationModel.Package.Current.DisplayName;

        public string Publisher => Windows.ApplicationModel.Package.Current.PublisherDisplayName;

        public string Version
        {
            get
            {
                var ver = Windows.ApplicationModel.Package.Current.Id.Version;
                return ver.Major.ToString() + "." + ver.Minor.ToString() + "." + ver.Build.ToString() + "." + ver.Revision.ToString();
            }
        }

        public Uri RateMe => new Uri("http://bing.com");
    }
}

