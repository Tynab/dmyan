namespace DMYAN.Scripts.Common.Enum;

internal enum DuelStep
{
    None = 0,               // Vô
    Drawing = 1,            // Đang rút bài
    Drawn = 2,              // Đã rút bài
    Activating = 3,         // Đang kích hoạt
    Activated = 4,          // Đã kích hoạt
    Setting = 5,            // Đang đặt bài
    Set = 6,                // Đã đặt bài
    Summoning = 7,          // Đang triệu hồi
    Summoned = 8,           // Đã triệu hồi
    SetSummoning = 9,       // Đang đặt triệu hồi
    SetSummoned = 10,       // Đã đặt triệu hồi
    FlipSummoning = 11,     // Đang lật triệu hồi
    FlipSummoned = 12,      // Đã lật triệu hồi
    SpecialSummoning = 13,  // Đang đặc biệt triệu hồi
    SpecialSummoned = 14,   // Đã đặc biệt triệu hồi
    Attacking = 15,         // Đang tấn công
    Attacked = 16,          // Đã tấn công
    Destroying = 17,        // Đang phá hủy
    Destroyed = 18          // Đã phá hủy
}
