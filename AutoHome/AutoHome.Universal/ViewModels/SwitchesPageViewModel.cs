using AutoHome.Universal.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Threading.Tasks;
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
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                _settings = Services.SettingsServices.SettingsService.Instance;

            _domoticzConnection = Services.DomoticzConnectionServices.DomoticzConnection.Instance;
            observableSwitches = new ObservableCollection<DomoticzLightSwitch>();
            

        }

        private ObservableCollection<DomoticzLightSwitch> _observableSwitches;
        public ObservableCollection<DomoticzLightSwitch> observableSwitches
        {
            get { return _observableSwitches; }
            set { _observableSwitches = value; base.RaisePropertyChanged(); }
        }

        public async void LoadSwitches(object sender, object parameter)
        {
            try
            {
                DomoticzGetAllSwitchesResponse  response = await _domoticzConnection.GetAllSwitches();
                foreach (DomoticzLightSwitch s in response.LightSwitches)
                {
                    observableSwitches.Add(s);
                }
            }
            catch
            {
                //TODO: error handling
            }
        }

    }
}

