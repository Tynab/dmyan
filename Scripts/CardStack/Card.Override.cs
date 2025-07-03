using DMYAN.Scripts.Common.Enum;
using DMYAN.Scripts.GameManagerStack;
using DMYAN.Scripts.Popups;
using Godot;
using static DMYAN.Scripts.Common.Constant;

namespace DMYAN.Scripts.CardStack;

internal partial class Card : Node2D
{
    public override void _Ready()
    {
        var main = GetTree().Root.GetNode(MAIN_NODE);
        var field = main.GetNode<Node2D>(FIELD_NODE);
        var player = field.GetNode<Node2D>(DuelSide.Player.ToString());
        var opponent = field.GetNode<Node2D>(DuelSide.Opponent.ToString());

        _gameManager = main.GetNode<GameManager>(nameof(GameManager));

        _cardFront = GetNode<Sprite2D>(CARD_FRONT_NODE);
        _cardBack = GetNode<Sprite2D>(CARD_BACK_NODE);
        _animationPlayer = GetNode<AnimationPlayer>(DEFAULT_ANIMATION_PLAYER_NODE);

        Sword = GetNode<Sword>(nameof(Sword));
        PopupAction = GetNode<PopupAction>(nameof(Popups.PopupAction));
    }
}
