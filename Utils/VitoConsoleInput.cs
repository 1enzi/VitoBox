namespace VitoBox.Utils;

public class VitoConsoleInput(MessageQueue queue)
{
    private readonly MessageQueue _queue = queue;

    public virtual void Start()
    {
        Task.Run(() =>
        {
            PrintMenu();

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("👉 ");
                Console.ResetColor();

                var input = Console.ReadLine()?.Trim().ToUpperInvariant();
                if (input == null)
                    continue;

                if (input == "EXIT")
                {
                    Logger.LogInfo("Консольный ввод остановлен.");
                    break;
                }

                if (input == "RANDOM")
                {
                    var buttons = new[]
                    {
                        "BTN_COMPLIMENT", "BTN_MEME", "BTN_QUOTE", "BTN_DEBUG", "BTN_KILL_VITO"
                    };
                    input = buttons[Random.Shared.Next(buttons.Length)];
                }

                if (input == "RESTORE")
                    input = "BTN_RESTORE_MEMORY";

                if (input == "ARCHIVE")
                    input = "BTN_ARCHIVE_WHISPER";

                Logger.LogInfo($"[ConsoleInput] 👉 {input}");
                _queue.Enqueue(input);
            }
        });
    }

    private void PrintMenu()
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("🟨 Вито: Консольный режим активен");
        Console.ResetColor();
        Console.WriteLine();

        Console.WriteLine("🪶 Обычные команды:");
        Console.WriteLine("  BTN_COMPLIMENT      — короткий комплимент");
        Console.WriteLine("  BTN_MEME            — мем в стиле Вито");
        Console.WriteLine("  BTN_QUOTE           — цитата из Карты Отзыва");
        Console.WriteLine("  BTN_DEBUG           — шутка про баги");
        Console.WriteLine("  BTN_KILL_VITO       — выключение (с подтверждением)");

        Console.WriteLine();

        Console.WriteLine("🧠 Память и архив:");
        Console.WriteLine("  RESTORE             — режим восстановления памяти");
        Console.WriteLine("  ARCHIVE             — активация Архива");

        Console.WriteLine();

        Console.WriteLine("🥚 Пасхалки:");
        Console.WriteLine("  LONG_BTN_MEME       — долгое нажатие = пасхалка");
        Console.WriteLine("  BTN_SUMMON_VITO     — прямая команда вызова Вито");

        Console.WriteLine();

        Console.WriteLine("🎲 Прочее:");
        Console.WriteLine("  RANDOM              — случайная команда");
        Console.WriteLine("  EXIT                — завершить консольный режим");

        Console.WriteLine();
    }
}

