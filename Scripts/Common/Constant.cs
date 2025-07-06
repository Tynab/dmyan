using DMYAN.Scripts.Common.Enum;
using Godot;

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

    internal const string PLAYER_DP_POPUP_ASSET_PATH = "res://Assets/Popups/Player/dp.png";
    internal const string PLAYER_SP_POPUP_ASSET_PATH = "res://Assets/Popups/Player/sp.png";
    internal const string PLAYER_M1_POPUP_ASSET_PATH = "res://Assets/Popups/Player/m1.png";
    internal const string PLAYER_BP_POPUP_ASSET_PATH = "res://Assets/Popups/Player/bp.png";
    internal const string PLAYER_M2_POPUP_ASSET_PATH = "res://Assets/Popups/Player/m2.png";
    internal const string PLAYER_EP_POPUP_ASSET_PATH = "res://Assets/Popups/Player/ep.png";

    internal const string OPPONENT_DP_POPUP_ASSET_PATH = "res://Assets/Popups/Opponent/dp.png";
    internal const string OPPONENT_SP_POPUP_ASSET_PATH = "res://Assets/Popups/Opponent/sp.png";
    internal const string OPPONENT_M1_POPUP_ASSET_PATH = "res://Assets/Popups/Opponent/m1.png";
    internal const string OPPONENT_BP_POPUP_ASSET_PATH = "res://Assets/Popups/Opponent/bp.png";
    internal const string OPPONENT_M2_POPUP_ASSET_PATH = "res://Assets/Popups/Opponent/m2.png";
    internal const string OPPONENT_EP_POPUP_ASSET_PATH = "res://Assets/Popups/Opponent/ep.png";

    internal const string POSITION_NODE_PATH = "position";
    internal const string GLOBAL_POSITION_NODE_PATH = "global_position";
    internal const string SCALE_NODE_PATH = "scale";
    internal const string ROTATION_NODE_PATH = "rotation";
    internal const string OPACITY_NODE_PATH = "modulate:a8";

    internal const int INITIAL_HAND_SIZE = 1;

    internal const int DEFAULT_CARD_INFO_SWAP = 1;

    internal const int STARTUP_DELAY = 100;
    internal const int ATTACK_DELAY = 250;
    internal const int PHASE_CHANGE_DELAY = 500;

    internal const int CARD_COLLISION_MASK = 2;

    internal const int BUTTON_UP_FONT_SIZE = 40;
    internal const int BUTTON_DOWN_FONT_SIZE = 35;

    internal const int OPACITY_MIN = 0;
    internal const int OPACITY_MAX = 255;

    internal static readonly Vector2 PHASE_SCALE_MIN = new(.1f, .1f);
    internal static readonly Vector2 PHASE_BUTTON_SCALE_MIN = new(.2f, 1f);

    internal const float CARD_HAND_SCALE = 1;

    internal const float CARD_HAND_W = 120 * CARD_HAND_SCALE;
    internal const float CARD_HAND_GAP_W = 15;
    internal const float HAND_AREA_MAX_W = 1920;

    internal const float CARD_HAND_Y = 480;
    internal const float CARD_HAND_RAISE_Y = 20;

    internal const float DEFAULT_ANIMATION_SPEED = .1f;
    internal const float PHASE_BUTTON_ANIMATION_SPEED = .05f;
    internal const float PHASE_ANIMATION_SPEED = .2f;

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
    internal const string INFO_LIFE_POINT_NODE = "LifePoint";
    internal const string INFO_HEALTH_NODE = "Health";
    internal const string INFO_TIMER_NODE = "Timer";

    internal const string MAIN_NODE = "Main";
    internal const string SLOT_COUNT_NODE = "Count";

    internal const string COLLIDER_PROPERTY = "collider";
    internal const string FONT_SIZE_PROPERTY = "font_size";

    internal const string CARD_FLIP_UP_ANIMATION = "card_flip_up";
    internal const string CARD_FLIP_DOWN_ANIMATION = "card_flip_down";

    internal const string CARD_SWAP_1_ANIMATION = "card_swap_1";
    internal const string CARD_SWAP_2_ANIMATION = "card_swap_2";

    internal const string FINISHED_SIGNAL = "finished";

    internal const string DEFAULT_PLAYER = "Atem";
    internal const string DEFAULT_OPPONENT = "Seto";

    internal const DuelSide STARTING_DUEL_SIDE = DuelSide.Opponent;
}
