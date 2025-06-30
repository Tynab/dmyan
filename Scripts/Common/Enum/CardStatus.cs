namespace DMYAN.Scripts.Common.Enum;

internal enum CardStatus
{
    None = 0,         // Vô
    Summoning = 1,   // Đang triệu hồi
    Summoned = 2,     // Đã triệu hồi
    Attacking = 3,    // Đang tấn công
    Attacked = 4,     // Đã tấn công
    Destroying = 5,  // Đang phá hủy
    Destroyed = 6,    // Đã phá hủy
    SpecialSummoning = 7, // Đang đặc biệt triệu hồi
    SpecialSummoned = 8, // Đã đặc biệt triệu hồi
    Activating = 9,   // Đang kích hoạt
    Activated = 10,   // Đã kích hoạt
}
