using System.IO.Ports;
using VitoBox.Models.Interfaces;
using VitoBox.Utils;

namespace VitoBox.Models;

public class FakeSerialPort : ISerialPort
{
    public event SerialDataReceivedEventHandler DataReceived
    {
        add { }
        remove { }
    }

    public void Open() => Logger.LogInfo("FAKE SerialPort Open()");
    public void Close() => Logger.LogInfo("FAKE SerialPort Close()");
    public bool IsOpen => true;
    public string ReadLine() => "BTN_COMPLIMENT";
    public void WriteLine(string data) => Logger.LogInfo($"FAKE Serial Write: {data}");
}
