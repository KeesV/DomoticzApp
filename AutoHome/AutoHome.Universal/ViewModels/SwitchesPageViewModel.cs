using AutoHome.Universal.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml.Navigation;

namespace AutoHome.Universal.ViewModels
{
    public class SwitchesPageViewModel : Mvvm.ViewModelBase
    {
        Services.SettingsServices.SettingsService _settings;
        Services.DomoticzConnectionServices.DomoticzConnection _domoticzConnection;

        //public ObservableCollection<DomoticzLightSwitch> observableSwitches = new ObservableCollection<DomoticzLightSwitch>();

        public SwitchesPageViewModel()
        {
            _domoticzConnection = Services.DomoticzConnectionServices.DomoticzConnection.Instance;
            observableSwitches = new ObservableCollection<Switch>();

            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                _settings = Services.SettingsServices.SettingsService.Instance;
                observableSwitches.Add(new Switch() { Name = "Test switch 1", Status = true });
            }
        }

        private ObservableCollection<Switch> _observableSwitches;
        public ObservableCollection<Switch> observableSwitches
        {
            get { return _observableSwitches; }
            set { _observableSwitches = value; base.RaisePropertyChanged(); }
        }

        public async void LoadSwitches(object sender, object parameter)
        {
            observableSwitches.Clear();
            try
            {
                DomoticzGetAllSwitchesResponse  response = await _domoticzConnection.GetAllSwitches();
                foreach (DomoticzLightSwitch s in response.LightSwitches)
                {
                    DomoticzGetDeviceResponse r = await _domoticzConnection.GetDevice(s.idx);

                    Switch sw = new Switch();
                    sw.Name = s.Name;
                    sw.Status = r.Result[0].Status == "On" ? true : false;
                    
                    observableSwitches.Add(sw);
                }
            }
            catch
            {
                //TODO: error handling
            }
        }

        public async void ToggleSwitch(object sender, object parameter)
        {
            var arg = parameter as Windows.UI.Xaml.Controls.ItemClickEventArgs;
            var item = arg.ClickedItem as Switch;
            observableSwitches[observableSwitches.IndexOf(item)].Status = !item.Status;

            //await new MessageDialog(item.Name).ShowAsync();
        }

    }

    public class Switch : INotifyPropertyChanged
    {
        public string Name { get; set; }

        private bool _Status = false;
        public bool Status
        {
            get
            {
                return _Status;
            }
            set
            {
                _Status = value;
                OnPropertyChanged("Status");
                OnPropertyChanged("IconImage"); //IconImage will also change when we update the status
                
            }
        }
        public string IconImage {
            get {
                if(Status == true)
                {
                    return "ms-appx:///Assets/Light48_On.png";
                } else
                {
                    return "ms-appx:///Assets/Light48_Off.png";
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

