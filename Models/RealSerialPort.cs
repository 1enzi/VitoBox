using System.IO.Ports;
using VitoBox.Models.Interfaces;

namespace VitoBox.Models;

public class RealSerialPort : ISerialPort
{
    private readonly SerialPort _port;

    public RealSerialPort(string portName, int baudRate)
    {
        _port = new SerialPort(portName, baudRate);
    }

    public event SerialDataReceivedEventHandler DataReceived
    {
        add => _port.DataReceived += value;
        remove => _port.DataReceived -= value;
    }

    public void Open() => _port.Open();
    public void Close() => _port.Close();
    public bool IsOpen => _port.IsOpen;
    public string ReadLine() => _port.ReadLine();
    public void WriteLine(string data) => _port.WriteLine(data);
}