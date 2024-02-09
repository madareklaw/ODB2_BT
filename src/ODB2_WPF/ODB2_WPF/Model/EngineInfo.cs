using CommunityToolkit.Mvvm.ComponentModel;

namespace ODB2_WPF.Model;

public partial class EngineInfo : ObservableObject
{
    [ObservableProperty] private double _engineRpm;
    [ObservableProperty] private double _intakeAirTemperature;
}