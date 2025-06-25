using Godot;
using static Godot.Vector2;

namespace DMYAN.Scripts.Common;

public static class Constant
{
    public const string CARDS_DATA_PATH = "res://Data/cards.csv";
    public const string DECKS_DATA_PATH = "res://Data/decks.csv";
    public const string CARD_BACK_ASSET_PATH = "res://Assets/card_back.jpg";
    public const string SUMMON_POPUP_ASSET_PATH = "res://Assets/Popups/summon.png";
    public const string SET_POPUP_ASSET_PATH = "res://Assets/Popups/set.png";
    public const string ACTIVATE_POPUP_ASSET_PATH = "res://Assets/Popups/activate.png";
    public const string ATK_POPUP_ASSET_PATH = "res://Assets/Popups/acttack.png";
    public const string DEF_POPUP_ASSET_PATH = "res://Assets/Popups/defense.png";
    public const string DP_POPUP_ASSET_PATH = "res://Assets/Popups/dp.png";
    public const string SP_POPUP_ASSET_PATH = "res://Assets/Popups/sp.png";
    public const string M1_POPUP_ASSET_PATH = "res://Assets/Popups/m1.png";
    public const string BP_POPUP_ASSET_PATH = "res://Assets/Popups/bp.png";
    public const string M2_POPUP_ASSET_PATH = "res://Assets/Popups/m2.png";
    public const string EP_POPUP_ASSET_PATH = "res://Assets/Popups/ep.png";
    public const string POSITION_NODE_PATH = "position";
    public const string GLOBAL_POSITION_NODE_PATH = "global_position";
    public const string SCALE_NODE_PATH = "scale";
    public const string ROTATION_NODE_PATH = "rotation";

    public static readonly Vector2 CARD_IN_SLOT_POSITION = Zero;

    public const int INITIAL_HAND_SIZE = 1;

    public const int DEFAULT_CARD_INFO_SWAP = 1;

    public const int PHASE_CHANGE_DELAY = 500;
    public const int STARTUP_DELAY = 100;

    public const int CARD_COLLISION_MASK = 1;

    public const int BUTTON_UP_FONT_SIZE = 40;
    public const int BUTTON_DOWN_FONT_SIZE = 35;

    public static readonly Vector2 CARD_IN_SLOT_SCALE = One;
    public static readonly Vector2 CARD_IN_HAND_SCALE = One;
    public const float CARD_HAND_SCALE = 1;
    public const float START_POPUP_PHASE_SCALE = .1f;

    public const float CARD_HAND_W = 120 * CARD_HAND_SCALE;
    public const float CARD_HAND_GAP_W = 15;
    public const float HAND_AREA_MAX_W = 1920;

    public const float HAND_POSITION_Y = 480;
    public const float CARD_HAND_RAISE_Y = 20;

    public const float DEFAULT_ANIMATION_SPEED = .1f;
    public const float PHASE_ANIMATION_SPEED = .2f;

    public const string DP_BUTTON_NODE = "DPButton";
    public const string SP_BUTTON_NODE = "SPButton";
    public const string M1_BUTTON_NODE = "M1Button";
    public const string BP_BUTTON_NODE = "BPButton";
    public const string M2_BUTTON_NODE = "M2Button";
    public const string EP_BUTTON_NODE = "EPButton";
    public const string SLOT_COUNT_NODE = "Count";
    public const string CARD_FRONT_NODE = "CardFront";
    public const string CARD_BACK_NODE = "CardBack";
    public const string CARD_INFO_SWAP_1_NODE = "CardSwap1";
    public const string CARD_INFO_SWAP_2_NODE = "CardSwap2";
    public const string CARD_INFO_HEADER_NODE = "Header";
    public const string CARD_INFO_DETAIL_NODE = "Detail";
    public const string POWER_ATK_NODE = "Atk";
    public const string POWER_DEF_NODE = "Def";
    public const string POWER_SLASH_NODE = "Slash";
    public const string INFO_NAME_NODE = "Name";
    public const string INFO_LP_NODE = "LP";
    public const string INFO_LIFE_POINT_NODE = "LifePoint";
    public const string INFO_TIMER_NODE = "Timer";
    public const string INFO_PLAYER_NODE = "PlayerInfo";
    public const string INFO_OPPONENT_NODE = "OpponentInfo";
    public const string ANIMATION_PLAYER_NODE = "AnimationPlayer";
    public const string SPRITE2D_PLAYER_NODE = "Sprite2D";
    public const string AREA2D_PLAYER_NODE = "Area2D";

    public const string FONT_SIZE_PROPERTY = "font_size";

    public const string CARD_FLIP_DOWN_ANIMATION = "card_flip_down";
    public const string CARD_DRAW_FLIP_ANIMATION = "card_draw_flip";
    public const string CARD_SWAP_1_ANIMATION = "card_swap_1";
    public const string CARD_SWAP_2_ANIMATION = "card_swap_2";

    public const string FINISHED_SIGNAL = "finished";

    public const string DEFAULT_PLAYER = "Atem";
    public const string DEFAULT_OPPONENT = "Seto";

    public const DuelSide STARTING_DUEL_SIDE = DuelSide.Player;
}

public enum DuelSide
{
    None = 0,         // Vô
    Player = 1,       // Người chơi
    Opponent = 2,     // Đối thủ
}

public enum DuelPhase
{
    None = 0,         // Vô
    Draw = 1,         // Rút bài
    Standby = 2,      // Chuẩn bị
    Main1 = 3,        // Chính 1
    Battle = 4,       // Giao tranh
    Main2 = 5,        // Chính 2
    End = 6           // Kết thúc
}

public enum CardActionType
{
    None = 0,         // Vô
    Summon = 1,       // Triệu hồi
    Set = 2,          // Đặt
    Activate = 3,     // Kích hoạt
}

public enum CardStatus
{
    None = 0,         // Vô
    InDeck = 1,       // Trong bộ bài
    InHand = 2,       // Trên tay
    InBoard = 3       // Trên sân
}

public enum CardZone
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

public enum CardFace
{
    None = 0,         // Vô
    FaceUp = 1,       // Ngửa
    FaceDown = 2      // Úp
}

public enum CardPosition
{
    None = 0,         // Vô
    Attack = 1,       // Công
    Defense = 2       // Thủ
}

public enum CardType
{
    None = 0,         // Vô
    Monster = 1,      // Quái
    Spell = 2,        // Phép
    Trap = 3          // Bẫy
}

public enum CardProperty
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

public enum CardBanlistStatus
{
    None = 0,         // Vô
    Forbidden = 1,    // Cấm
    Limited = 2,      // Hạn chế
    SemiLimited = 3   // Bán hạn chế
}

public enum CardEffectType
{
    None = 0,         // Vô
    Continuous = 1    // Liên tục
}

public enum MonsterAttribute
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

public enum MonsterRace
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

public enum MonsterSummonType
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

public enum PopupActionType
{
    None = 0,         // Vô
    Summon = 1,       // Triệu hồi
    Set = 2,          // Đặt
    Activate = 3,     // Kích hoạt
    Attack = 4,       // Tấn công
    Defense = 5      // Phòng thủ
}

public enum PopupPhaseType
{
    None = 0,         // Vô
    DP = 1,           // Rút bài
    SP = 2,           // Chuẩn bị
    M1 = 3,           // Chính 1
    BP = 4,           // Giao tranh
    M2 = 5,           // Chính 2
    EP = 6            // Kết thúc
}
