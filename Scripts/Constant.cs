namespace DMYAN.Scripts;

public static class Constant
{
    public const string CARDS_CSV_PATH = "res://Data/cards.csv";
    public const string DECKS_CSV_PATH = "res://Data/decks.csv";

    public const int INITIAL_HAND_SIZE = 5;

    public const float CARD_HAND_SCALE = .8f;

    public const float CARD_HAND_W = 120 * CARD_HAND_SCALE;
    public const float CARD_HAND_GAP_W = 5;
    public const float HAND_AREA_MAX_W = 1920;

    public const float HAND_POSITION_Y = 475;

    public const string MAIN_DECK_NODE = "MainDeck";
    public const string COUNT_LABEL_NODE = "Count";
    public const string CARD_FRONT_NODE = "CardFront";
    public const string CARD_BACK_NODE = "CardBack";
    public const string ATK_NODE = "Atk";
    public const string DEF_NODE = "Def";
    public const string SLASH_NODE = "Slash";

    public const string CARD_DRAW_FLIP_ANIMATION = "card_draw_flip";
}

public enum DuelSide
{
    None = 0,
    Player = 1,
    Opponent = 2,
}

public enum CardStatus
{
    None = 0,
    InDeck = 1,
    InHand = 2,
    InBoard = 3
}

public enum CardZone
{
    None = 0,
    MainDeck = 1,
    ExtraDeck = 2,
    Main = 3,
    STP = 4,
    Field = 5,
    Graveyard = 6,
    Banished = 7
}

public enum CardFace
{
    None = 0,
    FaceUp = 1,
    FaceDown = 2,
}

public enum CardPosition
{
    None = 0,
    Attack = 1,
    Defense = 2
}

public enum CardType
{
    None = 0,
    Monster = 1,
    Spell = 2,
    Trap = 3,
}

public enum CardProperty
{
    None = 0,
    Normal = 1,
    Continuous = 2,
    Equip = 3,
    Field = 4,
    QuickPlay = 5,
    Ritual = 6,
    Counter = 7,
    Effect = 8,
    Fusion = 9,
    Synchro = 10,
    Xyz = 11,
    Pendulum = 12,
    Link = 13,
    DarkSynchro = 14,
    Token = 15,
    LegendaryDragon = 16
}

public enum CardBanlistStatus
{
    None = 0,
    Forbidden = 1,
    Limited = 2,
    SemiLimited = 3
}

public enum CardEffectType
{
    None = 0,
    Continuous = 1
}

public enum MonsterAttribute
{
    None = 0,
    Dark = 1,
    Divine = 2,
    Earth = 3,
    Fire = 4,
    Light = 5,
    Water = 6,
    Wind = 7,
    Spell = 8,
    Trap = 9
}

public enum MonsterRace
{
    None = 0,
    Aqua = 1,
    Beast = 2,
    BeastWarrior = 3,
    Creator = 4,
    CreatorGod = 5,
    Cyberse = 6,
    Dinosaur = 7,
    DivineBeast = 8,
    Dragon = 9,
    Fairy = 10,
    Fiend = 11,
    Fish = 12,
    Insect = 13,
    Illusion = 14,
    Machine = 15,
    Plant = 16,
    Psychic = 17,
    Pyro = 18,
    Reptile = 19,
    Rock = 20,
    SeaSerpent = 21,
    Spellcaster = 22,
    Thunder = 23,
    Warrior = 24,
    WingedBeast = 25,
    Wyrm = 26,
    Zombie = 27
}

public enum MonsterSummonType
{
    None = 0,
    Normal = 1,
    Effect = 2,
    Ritual = 3,
    Fusion = 4,
    Synchro = 5,
    Xyz = 6,
    Pendulum = 7,
    Link = 8
}
