using System.Text.Json;
using VitoBox.Models;
using VitoBox.Utils;

var configJson = File.ReadAllText("appsettings.json");
var config = JsonSerializer.Deserialize<AppSettingsData>(configJson);

if (config == null)
{
    Console.WriteLine("Ошибка чтения конфигурации.");
    return;
}

var vito = VitoBoxBuilder
            .WithConfig(config)
            .UseFakeSerial()
            .EnableConsole()
            .UseMockApi()
            .Build();

await vito.StartAsync();
