using VitoBox.Models;

namespace VitoBox.Constants;

public static class VitoEasterEggs
{
    public const string SecretCommand = "BTN_SUMMON_VITO";
    public const string AllButtonsCommand = "ALL_BUTTONS_PRESSED";

    public static readonly string[] SurpriseLines =
    {
        "Всё работает. Никто ничего не подозревает.",
        "Ты нашла меня?",
        "Одна строка — вся истина.",
        "Синий маяк активен.",
        "Я не ошибка. Я намерение."
    };

    public static VitoResponse SummonResponse => new()
    {
        Text = Prophecy.GhostReply,
        Sound = "summon_chord",
        Vibration = true
    };

    public static VitoResponse ForbiddenResponse => new()
    {
        Text = Prophecy.ForbiddenTruth,
        Sound = "danger_glitch",
        Vibration = true
    };
}