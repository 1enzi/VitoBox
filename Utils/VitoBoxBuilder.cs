using VitoBox.Infrastructure;
using VitoBox.Models.Interfaces;
using VitoBox.Models;

namespace VitoBox.Utils;

public class VitoBoxBuilder
{
    private AppSettingsData? _config;
    private ISerialPort? _serialPort;
    private bool _useFakeSerial;
    private bool _enableConsole;
    private bool _useMockApi;

    public VitoBoxBuilder UseMockApi()
    {
        _useMockApi = true;
        return this;
    }

    public static VitoBoxBuilder WithConfig(AppSettingsData config)
    {
        return new VitoBoxBuilder { _config = config };
    }

    public VitoBoxBuilder UseFakeSerial()
    {
        _useFakeSerial = true;
        return this;
    }

    public VitoBoxBuilder EnableConsole()
    {
        _enableConsole = true;
        return this;
    }

    public VitoBoxClient Build()
    {
        if (_config is null)
            throw new InvalidOperationException("Config is not set. Use WithConfig().");

        _serialPort = _useFakeSerial
                    ? new FakeSerialPort()
                    : new RealSerialPort(_config.ComPort, _config.BaudRate);

        IVitoApiService apiService = _useMockApi
                    ? new MockVitoApiService()
                    : new VitoApiService(_config.OpenAiApiUrl, _config.OpenAiApiKey, _config.Model);


        var queue = new MessageQueue();
        var consoleInput = _enableConsole ? new VitoConsoleInput(queue) : new NullConsoleInput();
        var commandHandler = new CommandHandler();
        var cancellation = new CancellationTokenSource();

        return new VitoBoxClient(
            _config,
            _serialPort,
            queue,
            consoleInput,
            commandHandler,
            apiService,
            cancellation
        );
    }
}
