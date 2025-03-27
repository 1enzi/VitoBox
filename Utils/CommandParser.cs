using VitoBox.Constants;
using VitoBox.Models.Enums;

namespace VitoBox.Utils;

public static class CommandParser
{
    public static (VitoCommandType, string?) Preprocess(string? raw)
    {
        #region 🧼 Очистка ввода
        if (string.IsNullOrWhiteSpace(raw))
            return (VitoCommandType.Unknown, null);

        raw = raw.Trim().ToUpperInvariant();
        #endregion

        #region ⏳ Долгое нажатие
        if (raw.StartsWith("LONG_"))
        {
            Logger.LogInfo($"Долгое нажатие обработано: {raw}");
            return (VitoCommandType.EasterEgg, null);
        }
        #endregion

        #region 🐣 Пасхалки
        if (raw == VitoEasterEggs.SecretCommand)
        {
            Logger.LogInfo("Обработана пасхальная команда вызова");
            return (VitoCommandType.EasterEgg, null);
        }

        if (raw == VitoEasterEggs.AllButtonsCommand)
        {
            Logger.LogError("Запрещённая комбинация! Архив гневается.");
            throw new Exception(Prophecy.ForbiddenTruth);
        }
        #endregion

        #region 🔁 Пропуск к обычной обработке
        return (VitoCommandType.Unknown, raw);
        #endregion
    }
}
