using System;
using Windows.UI.Xaml;

namespace AutoHome.Universal.Services.SettingsServices
{
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-SettingsService
    public partial class SettingsService : ISettingsService
    {
        public static SettingsService Instance { get; }
        static SettingsService()
        {
            // implement singleton pattern
            Instance = Instance ?? new SettingsService();
        }

        Template10.Services.SettingsService.ISettingsHelper _helper;
        private SettingsService()
        {
            _helper = new Template10.Services.SettingsService.SettingsHelper();
        }

        public bool UseShellBackButton
        {
            get { return _helper.Read<bool>(nameof(UseShellBackButton), true); }
            set
            {
                _helper.Write(nameof(UseShellBackButton), value);
                ApplyUseShellBackButton(value);
            }
        }

        public ApplicationTheme AppTheme
        {
            get
            {
                var theme = ApplicationTheme.Dark;
                var value = _helper.Read<string>(nameof(AppTheme), theme.ToString());
                return Enum.TryParse<ApplicationTheme>(value, out theme) ? theme : ApplicationTheme.Dark;
            }
            set
            {
                _helper.Write(nameof(AppTheme), value.ToString());
                ApplyAppTheme(value);
            }
        }

        public TimeSpan CacheMaxDuration
        {
            get { return _helper.Read<TimeSpan>(nameof(CacheMaxDuration), TimeSpan.FromDays(2)); }
            set
            {
                _helper.Write(nameof(CacheMaxDuration), value);
                ApplyCacheMaxDuration(value);
            }
        }

        public string DomoticzServer
        {
            get { return _helper.Read<string>(nameof(DomoticzServer), "192.168.1.4"); }
            set
            {
                _helper.Write(nameof(DomoticzServer), value);
                //Put anything else that needs to happen when the domoticz server is updated here
            }
        }

        public int DomoticzPort
        {
            get { return _helper.Read<int>(nameof(DomoticzPort), 8080); }
            set
            {
                _helper.Write(nameof(DomoticzPort), value);
                //Put anything else that needs to happen when the domoticz port is updated here
            }
        }
    }
}

