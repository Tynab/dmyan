namespace DMYAN.Scripts;

public static class Constant
{
    #region Collision Masks

    public const int CARD_COLLISION_MASK = 1; // Layer 1 (2^(1-1) = 1)
    public const int CARD_SLOT_V_COLLISION_MASK = 2; // Layer 2 (2^(2-1) = 2)
    public const int CARD_SLOT_H_COLLISION_MASK = 4; // Layer 3 (2^(3-1) = 4)
    public const int DECK_COLLISION_MASK = 8; // Layer 4 (2^(4-1) = 8)

    #endregion

    #region Dimensions

    public const int CARD_W = 80;
    public const int HAND_Y = 820;
    public const int STARTING_HAND_SIZE = 5;
    public const float CARD_DEFAULT_SCALE = 1;
    public const float CARD_HOVERED_SCALE = 1.05f;

    #endregion

    #region Animation Speeds

    public const double CARD_DEFAULT_ANIMATION_SPEED = 0.1;
    public const double CARD_DRAW_ANIMATION_SPEED = 0.2;

    #endregion

    #region Animation Layers

    public const string CARD_FLIP_ANIMATION_LAYER = "card_flip";

    #endregion

    #region Paths

    public const string CARD_BACK_ASSET_PATH = "res://Assets/card_back.jpg";
    public const string CARD_SCENE_PATH = "res://Scenes/Card.tscn";

    #endregion
}
