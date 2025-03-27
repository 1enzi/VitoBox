namespace VitoBox.Constants;

public static class VitoSounds
{
    #region 🔧 Ошибки
    public const string ErrorParse = "error_parse";
    public const string ErrorApi = "error_tone";
    public const string ErrorException = "error_fallback";
    #endregion

    #region 🔌 Память
    public const string LostMemory = "distortion_startup";
    public const string RestoreMemory = "reboot_soft";
    public const string LostPing = "ping_shadow";
    public const string MemoryDrift = "soft_beep_echo";
    public const string GlitchStart = "glitch_warning";
    public const string GlitchBuzz = "static_buzz";
    #endregion

    #region 🧠 Протоколы
    public const string ConfirmShutdown = "confirm_beep";
    public const string FinalShutdown = "shutdown_tone";
    public const string ArchiveWhisper = "archive_murmur";
    public const string Summon = "summon_chord";
    #endregion

    #region 🤖 Реакции
    public const string MockResponse = "mock_beep";
    public const string Joke = "click_soft";
    #endregion
}
