using System.Collections.ObjectModel;
using System.IO.Ports;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OBD.NET.OBDData;
using ODB2_WPF.Model;

namespace ODB2_WPF.ViewModel;

public partial class MainWindowViewModel : ObservableObject
{
    private OBDWrapper _obdWrapper;

    [ObservableProperty] private string _selectedPort = string.Empty;

    [ObservableProperty]private EngineInfo _engineInfo = new EngineInfo();

    public MainWindowViewModel()
    {
        _obdWrapper = new();

        GetSerialPorts();
    }

    public ObservableCollection<string> SerialPorts { get; } = new();

    [RelayCommand]
    private void GetSerialPorts()
    {
        SerialPorts.Clear();

        foreach (var port in SerialPort.GetPortNames())
        {
            SerialPorts.Add(port);
        }
    }

    [RelayCommand]
    private void Connect()
    {
        _obdWrapper.UpdateComms(SelectedPort);

        SubscribeToEngine(EngineInfo);
    }

    private void SubscribeToEngine(EngineInfo model)
    {
        _obdWrapper.SubscribeToData<EngineRPM>((o, rpm) => { model.EngineRpm = rpm.Data.Rpm.Value;});
        _obdWrapper.SubscribeToData<IntakeAirTemperature>((o, rpm) => { model.IntakeAirTemperature = rpm.Data.Temperature; });
    }
    
    
}