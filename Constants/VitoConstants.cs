namespace VitoBox.Constants;

public static class VitoConstants
{
    #region OpenAI
    public const string SystemPrompt =
        "Ты Вито. Отвечай в JSON формате: { \"text\": \"...\", \"sound\": \"...\", \"vibration\": true }";
    #endregion

    #region Serial output commands
    public const string OutDisplay = "OUT:DISPLAY:";
    public const string OutSound = "OUT:SOUND:";
    public const string OutVibrate = "OUT:VIBRATE:ON";
    #endregion

    #region Delay
    public const int ShutdownDelayMs = 1500;
    #endregion
}
