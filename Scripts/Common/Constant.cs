using Godot;
using static Godot.Vector2;

namespace DMYAN.Scripts.Common;

internal static class Constant
{
    internal const string CARDS_DATA_PATH = "res://Data/cards.csv";
    internal const string DECKS_DATA_PATH = "res://Data/decks.csv";
    internal const string CARD_BACK_ASSET_PATH = "res://Assets/card_back.jpg";
    internal const string SUMMON_POPUP_ASSET_PATH = "res://Assets/Popups/summon.png";
    internal const string SET_POPUP_ASSET_PATH = "res://Assets/Popups/set.png";
    internal const string ACTIVATE_POPUP_ASSET_PATH = "res://Assets/Popups/activate.png";
    internal const string ATK_POPUP_ASSET_PATH = "res://Assets/Popups/acttack.png";
    internal const string DEF_POPUP_ASSET_PATH = "res://Assets/Popups/defense.png";
    internal const string DP_POPUP_ASSET_PATH = "res://Assets/Popups/dp.png";
    internal const string SP_POPUP_ASSET_PATH = "res://Assets/Popups/sp.png";
    internal const string M1_POPUP_ASSET_PATH = "res://Assets/Popups/m1.png";
    internal const string BP_POPUP_ASSET_PATH = "res://Assets/Popups/bp.png";
    internal const string M2_POPUP_ASSET_PATH = "res://Assets/Popups/m2.png";
    internal const string EP_POPUP_ASSET_PATH = "res://Assets/Popups/ep.png";
    internal const string POSITION_NODE_PATH = "position";
    internal const string GLOBAL_POSITION_NODE_PATH = "global_position";
    internal const string SCALE_NODE_PATH = "scale";
    internal const string ROTATION_NODE_PATH = "rotation";

    internal static readonly Vector2 CARD_IN_SLOT_POSITION = Zero;

    internal const int INITIAL_HAND_SIZE = 1;

    internal const int DEFAULT_CARD_INFO_SWAP = 1;

    internal const int PHASE_CHANGE_DELAY = 500;
    internal const int STARTUP_DELAY = 100;

    internal const int CARD_COLLISION_MASK = 1;

    internal const int BUTTON_UP_FONT_SIZE = 40;
    internal const int BUTTON_DOWN_FONT_SIZE = 35;

    internal static readonly Vector2 CARD_IN_SLOT_SCALE = One;
    internal static readonly Vector2 CARD_IN_HAND_SCALE = One;
    internal const float CARD_HAND_SCALE = 1;
    internal const float START_POPUP_PHASE_SCALE = .1f;

    internal const float CARD_HAND_W = 120 * CARD_HAND_SCALE;
    internal const float CARD_HAND_GAP_W = 15;
    internal const float HAND_AREA_MAX_W = 1920;

    internal const float HAND_POSITION_Y = 480;
    internal const float CARD_HAND_RAISE_Y = 20;

    internal const float DEFAULT_ANIMATION_SPEED = .1f;
    internal const float PHASE_ANIMATION_SPEED = .2f;

    internal const string DP_BUTTON_NODE = "DPButton";
    internal const string SP_BUTTON_NODE = "SPButton";
    internal const string M1_BUTTON_NODE = "M1Button";
    internal const string BP_BUTTON_NODE = "BPButton";
    internal const string M2_BUTTON_NODE = "M2Button";
    internal const string EP_BUTTON_NODE = "EPButton";
    internal const string SLOT_COUNT_NODE = "Count";
    internal const string CARD_FRONT_NODE = "CardFront";
    internal const string CARD_BACK_NODE = "CardBack";
    internal const string CARD_INFO_SWAP_1_NODE = "CardSwap1";
    internal const string CARD_INFO_SWAP_2_NODE = "CardSwap2";
    internal const string CARD_INFO_HEADER_NODE = "Header";
    internal const string CARD_INFO_DETAIL_NODE = "Detail";
    internal const string POWER_ATK_NODE = "Atk";
    internal const string POWER_DEF_NODE = "Def";
    internal const string POWER_SLASH_NODE = "Slash";
    internal const string INFO_NAME_NODE = "Name";
    internal const string INFO_LP_NODE = "LP";
    internal const string INFO_LIFE_POINT_NODE = "LifePoint";
    internal const string INFO_TIMER_NODE = "Timer";
    internal const string INFO_PLAYER_NODE = "PlayerInfo";
    internal const string INFO_OPPONENT_NODE = "OpponentInfo";
    internal const string FIELD_PLAYER_NODE = "PlayerField";
    internal const string FIELD_OPPONENT_NODE = "OpponentField";
    internal const string BOARD_NODE = "Board";

    internal const string DEFAULT_ANIMATION_PLAYER_NODE = "AnimationPlayer";
    internal const string DEFAULT_SPRITE2D_NODE = "Sprite2D";
    internal const string DEFAULT_AREA2D_NODE = "Area2D";

    internal const string COLLIDER_PROPERTY = "collider";
    internal const string FONT_SIZE_PROPERTY = "font_size";

    internal const string CARD_FLIP_DOWN_ANIMATION = "card_flip_down";
    internal const string CARD_DRAW_FLIP_ANIMATION = "card_draw_flip";
    internal const string CARD_SWAP_1_ANIMATION = "card_swap_1";
    internal const string CARD_SWAP_2_ANIMATION = "card_swap_2";

    internal const string FINISHED_SIGNAL = "finished";

    internal const string DEFAULT_PLAYER = "Atem";
    internal const string DEFAULT_OPPONENT = "Seto";

    internal const DuelSide STARTING_DUEL_SIDE = DuelSide.Player;
}

internal enum DuelSide
{
    None = 0,         // Vô
    Player = 1,       // Người chơi
    Opponent = 2,     // Đối thủ
}

internal enum DuelPhase
{
    None = 0,         // Vô
    Draw = 1,         // Rút bài
    Standby = 2,      // Chuẩn bị
    Main1 = 3,        // Chính 1
    Battle = 4,       // Giao tranh
    Main2 = 5,        // Chính 2
    End = 6           // Kết thúc
}

internal enum CardActionType
{
    None = 0,         // Vô
    Summon = 1,       // Triệu hồi
    Set = 2,          // Đặt
    Activate = 3,     // Kích hoạt
}

internal enum CardStatus
{
    None = 0,         // Vô
    InDeck = 1,       // Trong bộ bài
    InHand = 2,       // Trên tay
    InBoard = 3       // Trên sân
}

internal enum CardZone
{
    None = 0,         // Vô
    MainDeck = 1,     // Bộ bài chính
    ExtraDeck = 2,    // Bộ bài phụ
    Main = 3,         // Chính khu
    STP = 4,          // Phép trận
    Field = 5,        // Dã khu
    Graveyard = 6,    // Mộ địa
    Banished = 7      // Loại trừ
}

internal enum CardFace
{
    None = 0,         // Vô
    FaceUp = 1,       // Ngửa
    FaceDown = 2      // Úp
}

internal enum CardPosition
{
    None = 0,         // Vô
    Attack = 1,       // Công
    Defense = 2       // Thủ
}

internal enum CardType
{
    None = 0,         // Vô
    Monster = 1,      // Quái
    Spell = 2,        // Phép
    Trap = 3          // Bẫy
}

internal enum CardProperty
{
    None = 0,             // Vô
    Normal = 1,           // Thường
    Continuous = 2,       // Liên tục
    Equip = 3,            // Trang bị
    Field = 4,            // Sân dã
    QuickPlay = 5,        // Cấp tốc
    Ritual = 6,           // Nghi lễ
    Counter = 7,          // Phản kích
    Effect = 8,           // Hiệu ứng
    Fusion = 9,           // Dung hợp
    Synchro = 10,         // Đồng bộ
    Xyz = 11,             // Chồng lớp
    Pendulum = 12,        // Dao động
    Link = 13,            // Liên kết
    DarkSynchro = 14,     // Hắc đồng bộ
    Token = 15,           // Ảo ảnh
    LegendaryDragon = 16  // Truyền kỳ long
}

internal enum CardBanlistStatus
{
    None = 0,         // Vô
    Forbidden = 1,    // Cấm
    Limited = 2,      // Hạn chế
    SemiLimited = 3   // Bán hạn chế
}

internal enum CardEffectType
{
    None = 0,         // Vô
    Continuous = 1    // Liên tục
}

internal enum MonsterAttribute
{
    None = 0,     // Vô
    Dark = 1,     // Ám
    Divine = 2,   // Thần
    Earth = 3,    // Địa
    Fire = 4,     // Hỏa
    Light = 5,    // Quang
    Water = 6,    // Thủy
    Wind = 7,     // Phong
    Spell = 8,    // Phép
    Trap = 9      // Bẫy
}

internal enum MonsterRace
{
    None = 0,         // Vô
    Aqua = 1,         // Thủy sinh
    Beast = 2,        // Thú
    BeastWarrior = 3, // Thú chiến
    Creator = 4,      // Tạo giả
    CreatorGod = 5,   // Sáng thế thần
    Cyberse = 6,      // Không gian mạng
    Dinosaur = 7,     // Cự long
    DivineBeast = 8,  // Thần thú
    Dragon = 9,       // Long
    Fairy = 10,       // Tiên
    Fiend = 11,       // Ác ma
    Fish = 12,        // Ngư
    Insect = 13,      // Trùng
    Illusion = 14,    // Ảo ảnh
    Machine = 15,     // Cơ
    Plant = 16,       // Thực vật
    Psychic = 17,     // Tâm linh
    Pyro = 18,        // Viêm
    Reptile = 19,     // Bò sát
    Rock = 20,        // Thạch
    SeaSerpent = 21,  // Hải xà
    Spellcaster = 22, // Pháp sư
    Thunder = 23,     // Lôi điện
    Warrior = 24,     // Chiến binh
    WingedBeast = 25, // Dực thú
    Wyrm = 26,        // Á long
    Zombie = 27       // Thây ma
}

internal enum MonsterSummonType
{
    None = 0,     // Vô
    Normal = 1,   // Thường triệu
    Effect = 2,   // Hiệu triệu
    Ritual = 3,   // Nghi triệu
    Fusion = 4,   // Dung triệu
    Synchro = 5,  // Đồng triệu
    Xyz = 6,      // Chồng triệu
    Pendulum = 7, // Dao triệu
    Link = 8      // Liên triệu
}

internal enum PopupActionType
{
    None = 0,         // Vô
    Summon = 1,       // Triệu hồi
    Set = 2,          // Đặt
    Activate = 3,     // Kích hoạt
    Attack = 4,       // Tấn công
    Defense = 5      // Phòng thủ
}

internal enum PopupPhaseType
{
    None = 0,         // Vô
    DP = 1,           // Rút bài
    SP = 2,           // Chuẩn bị
    M1 = 3,           // Chính 1
    BP = 4,           // Giao tranh
    M2 = 5,           // Chính 2
    EP = 6            // Kết thúc
}
