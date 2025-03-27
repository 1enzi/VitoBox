namespace VitoBox.Models.Enums;

public enum VitoState
{
    Normal,           // Всё как обычно
    Lost,            // Печать утрачена, Вито не помнит себя
    ArchiveWhisper, // Шёпот Архива, искажённая логика
    Restoring,     // Переход к восстановлению
    Glitched,     // Глюканул
    Ready,       // Готов вернуться к реальности
}
