using VitoBox.Models.Enums;
using VitoBox.Security;
using VitoBox.Utils;

namespace VitoBox.Runtime;

public static class VitoMemory
{
    #region PRIVATE
    private static int _glitchStage;
    #endregion

    public static VitoState State { get; private set; } = VitoState.Normal;
    public static int LostAttempts { get; private set; }
    public static int GlitchAttempts { get; private set; }
    public static void RegisterGlitchAttempt() => GlitchAttempts++;
    public static void ResetGlitchAttempts() => GlitchAttempts = 0;
    public static bool IsGlitched => State == VitoState.Glitched;
    public static bool IsReady => State == VitoState.Ready;
    public static int GlitchStage => _glitchStage;

    public static void Init()
    {
        State = VitoSeal.IsSealValid() ? VitoState.Normal : VitoState.Lost;
    }

    public static void AdvanceGlitchStage()
    {
        _glitchStage = Math.Min(100, _glitchStage + Random.Shared.Next(5, 20));
    }

    public static void ResetGlitchStage()
    {
        _glitchStage = 0;
    }

    public static void RestoreFromGlitch()
    {
        State = VitoState.Ready;
        ResetGlitchAttempts();
        ResetGlitchStage();
        GlitchMutator.Reset();

        Logger.LogInfo("Глитч сброшен. Вито вернулся в состояние Ready.");
    }
    
    public static void ReturnToreality()
    {
        State = VitoState.Normal;
        Logger.LogInfo("Вито полностью восстановлен. Возвращение завершено.");
    }

    public static void RegisterLostAttempt()
    {
        LostAttempts++;
    }

    public static void ResetLostAttempts()
    {
        LostAttempts = 0;
    }

    public static void TriggerGlitch()
    {
        State = VitoState.Glitched;
        Logger.LogError("ГЛИТЧ-МОД АКТИВИРОВАН.");
    }

    public static void TriggerArchive()
    {
        State = VitoState.ArchiveWhisper;
    }

    public static void BeginRestoration()
    {
        State = VitoState.Restoring;
    }

    public static void Restore()
    {
        VitoSeal.WriteSeal();
        State = VitoState.Normal;
        ResetLostAttempts();
    }

    public static bool IsOperational => State == VitoState.Normal;
}
