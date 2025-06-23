namespace DMYAN.Scripts;

public static class Constant
{
    public const string CARDS_DATA_PATH = "res://Data/cards.csv";
    public const string DECKS_DATA_PATH = "res://Data/decks.csv";
    public const string CARD_BACK_ASSET_PATH = "res://Assets/card_back.jpg";
    public const string SUMMON_POPUP_ASSET_PATH = "res://Assets/Popup/summon.png";
    public const string SET_POPUP_ASSET_PATH = "res://Assets/Popup/set.png";
    public const string ACTIVE_POPUP_ASSET_PATH = "res://Assets/Popup/active.png";
    public const string ATK_POPUP_ASSET_PATH = "res://Assets/Popup/acttack.png";
    public const string DEF_POPUP_ASSET_PATH = "res://Assets/Popup/defense.png";
    public const string POSITION_NODE_PATH = "position";
    public const string GLOBAL_POSITION_NODE_PATH = "global_position";
    public const string SCALE_NODE_PATH = "scale";

    public const int INITIAL_HAND_SIZE = 1;

    public const int DEFAULT_CARD_INFO_SWAP = 1;

    public const int PHASE_CHANGE_DELAY = 1000;
    public const int STARTUP_DELAY = 100;

    public const int CARD_COLLISION_MASK = 1;

    public const float CARD_HAND_SCALE = 1;

    public const float CARD_HAND_W = 120 * CARD_HAND_SCALE;
    public const float CARD_HAND_GAP_W = 15;
    public const float HAND_AREA_MAX_W = 1920;

    public const float HAND_POSITION_Y = 480;
    public const float CARD_HAND_RAISE_Y = 20;

    public const float DEFAULT_ANIMATION_SPEED = .1f;

    public const string DP_BUTTON_NODE = "DP";
    public const string SP_BUTTON_NODE = "SP";
    public const string M1_BUTTON_NODE = "M1";
    public const string EP_BUTTON_NODE = "EP";
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
    public const string DEFAULT_ANIMATION_PLAYER_NODE = "AnimationPlayer";

    public const string CARD_DRAW_FLIP_ANIMATION = "card_draw_flip";
    public const string CARD_SWAP_1_ANIMATION = "card_swap_1";
    public const string CARD_SWAP_2_ANIMATION = "card_swap_2";

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
