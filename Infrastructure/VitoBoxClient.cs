using System.IO.Ports;
using VitoBox.Constants;
using VitoBox.Models;
using VitoBox.Models.Enums;
using VitoBox.Models.Interfaces;
using VitoBox.Runtime;
using VitoBox.Utils;

namespace VitoBox.Infrastructure;

public class VitoBoxClient
{
    private readonly AppSettingsData _config;
    private readonly ISerialPort _serial;
    private readonly MessageQueue _queue;
    private readonly VitoConsoleInput _consoleInput;
    private readonly CommandHandler _commandHandler;
    private readonly IVitoApiService _apiService;
    private readonly CancellationTokenSource _cancellationToken;

    public VitoBoxClient(AppSettingsData config)
    {
        _config = config;
        _serial = SerialFactory.Create(config);
        _queue = new MessageQueue();
        _consoleInput = new VitoConsoleInput(_queue);
        _commandHandler = new CommandHandler();
        _apiService = new VitoApiService(config.OpenAiApiUrl, config.OpenAiApiKey, config.Model);
        _cancellationToken = new CancellationTokenSource();
    }

    public VitoBoxClient(AppSettingsData config, 
                         ISerialPort serial, 
                         MessageQueue queue, 
                         VitoConsoleInput consoleInput, 
                         CommandHandler commandHandler, 
                         IVitoApiService apiService, 
                         CancellationTokenSource cancellationToken)
    {
        _config = config;
        _serial = serial;
        _queue = queue;
        _consoleInput = consoleInput;
        _commandHandler = commandHandler;
        _apiService = apiService;
        _cancellationToken = cancellationToken;

    }

    public async Task StartAsync()
    {
        _consoleInput.Start();

        Logger.LogInfo("VitoBoxClient starting...");
        _serial.DataReceived += OnSerialData;
        _serial.Open();
        Logger.LogInfo($"COM-порт {_config.ComPort} открыт.");

        await InitializeAsync();
        await Task.Run(() => RunLoopAsync(_cancellationToken.Token));
    }

    private async Task InitializeAsync()
    {
        VitoMemory.Init();

        var startupLine = VitoEasterEggs.SurpriseLines[Random.Shared.Next(VitoEasterEggs.SurpriseLines.Length)];
        _serial.WriteLine($"{VitoConstants.OutDisplay}{startupLine}");

        if (!RememberWhoYouAre())
            return;

        Logger.LogInfo(VitoEasterEggs.SurpriseLines[Random.Shared.Next(VitoEasterEggs.SurpriseLines.Length)]);
        await Task.CompletedTask;
    }

    private async Task RunLoopAsync(CancellationToken token)
    {
        var executor = new VitoCommandExecutor(_serial, _apiService);

        while (!token.IsCancellationRequested)
        {
            if (_queue.TryDequeue(out var raw))
            {
                var (type, prompt) = _commandHandler.Parse(raw);

                try
                {
                    await executor.ExecuteAsync(type, prompt, token);
                }
                catch (OperationCanceledException)
                {
                    Stop();
                    return;
                }
            }

            try
            {
                await Task.Delay(_config.QueueCheckIntervalMs, token);
            }
            catch (TaskCanceledException)
            {
                Logger.LogInfo("WorkerLoop остановлен по запросу.");
                break;
            }
        }
    }

    private void OnSerialData(object sender, SerialDataReceivedEventArgs e)
    {
        try
        {
            var line = _serial.ReadLine()?.Trim();
            if (line != null)
            {
                Logger.LogInfo($"Получено: {line}");
                _queue.Enqueue(line);
            }
        }
        catch (Exception ex)
        {
            Logger.LogError($"Ошибка при чтении: {ex.Message}");
        }
    }

    public void Stop()
    {
        _cancellationToken.Cancel();

        if (_serial.IsOpen)
            _serial.Close();

        Logger.LogInfo("VitoBoxClient остановлен.");
    }

    private bool RememberWhoYouAre()
    {
        if (VitoMemory.State == VitoState.Lost)
        {
            Logger.LogWarning("Печать Вито утрачена. Он не помнит, кто он.");

            var display = VitoPhrases.LostMemoryDisplay;
            var sound = VitoSounds.LostMemory;

            if (Random.Shared.Next(0, 4) == 0)
                display = VitoPhrases.AmnesiaLines[Random.Shared.Next(VitoPhrases.AmnesiaLines.Length)];

            _serial.WriteLine($"{VitoConstants.OutDisplay}{display}");
            _serial.WriteLine($"{VitoConstants.OutSound}{sound}");
            _serial.WriteLine($"{VitoConstants.OutVibrate}");

            return false;
        }

        return true;
    }
}
