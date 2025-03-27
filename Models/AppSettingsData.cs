namespace VitoBox.Models;

public class AppSettingsData
{
    public string OpenAiApiKey { get; set; } = "";
    public string OpenAiApiUrl { get; set; } = "";
    public string ComPort { get; set; } = "COM3";
    public int BaudRate { get; set; } = 115200;
    public string Model { get; set; } = "gpt-3.5-turbo";
    public int QueueCheckIntervalMs { get; set; } = 100;
    public bool UseFakeSerialPort { get; set; }
}
