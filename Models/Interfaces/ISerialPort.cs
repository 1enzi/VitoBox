using System.IO.Ports;

namespace VitoBox.Models.Interfaces;

public interface ISerialPort
{
    event SerialDataReceivedEventHandler DataReceived;
    void Open();
    void Close();
    bool IsOpen { get; }
    string ReadLine();
    void WriteLine(string data);
}
