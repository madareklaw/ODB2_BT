using OBD.NET.Communication;
using OBD.NET.Devices;
using OBD.NET.Logging;
using OBD.NET.OBDData;


namespace ODB2_WPF;

public class OBDWrapper : IDisposable
{
    private SerialConnection? _connection;
    private ELM327? _device;

    public void UpdateComms(string serialPort)
    {

        if (_device is not null)
        {
            _device.Dispose(true);
            _device = null;
        }
        if (_connection is not null)
        {
            _connection.Dispose();
            _connection = null;
        }

        _connection = new SerialConnection(serialPort);
        _device = new ELM327(_connection, new OBDDebugLogger(OBDLogLevel.Debug));

        _device.Initialize();
    }

    public void SubscribeToData<T>(ELM327.DataReceivedEventHandler<T> eventHandler) where T : IOBDData
    {
        if (_device is null)
        {
            throw new ArgumentNullException(nameof(_device));
        }

        _device.SubscribeDataReceived(eventHandler);
    }

    public void GetData<T>() where T : class, IOBDData, new()
    {
        _device?.RequestData<T>();
    }

    public void Close()
    {
        _device?.Dispose();
        _device = null;
        _connection?.Dispose();
        _connection = null;
    }

    public void Dispose()
    {
        Close();
    }
}