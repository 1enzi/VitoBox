using VitoBox.Models.Interfaces;
using VitoBox.Models;

namespace VitoBox.Utils;

public static class SerialFactory
{
    public static ISerialPort Create(AppSettingsData config)
    {
        return config.UseFakeSerialPort
            ? new FakeSerialPort()
            : new RealSerialPort(config.ComPort, config.BaudRate);
    }
}