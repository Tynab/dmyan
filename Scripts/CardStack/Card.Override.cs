using DMYAN.Scripts.GameManagerStack;
using DMYAN.Scripts.Popups;
using DMYAN.Scripts.SwordStack;
using Godot;
using static DMYAN.Scripts.Common.Constant;

namespace DMYAN.Scripts.CardStack;

internal partial class Card : Node2D
{
    public override void _Ready()
    {
        var main = GetTree().Root.GetNode(MAIN_NODE);

        _gameManager = main.GetNode<GameManager>(nameof(GameManager));

        _cardFront = GetNode<Sprite2D>(CARD_FRONT_NODE);
        _cardBack = GetNode<Sprite2D>(CARD_BACK_NODE);
        _animationPlayer = GetNode<AnimationPlayer>(nameof(AnimationPlayer));

        Sword = GetNode<Sword>(nameof(Sword));
        PopupAction = GetNode<PopupAction>(nameof(Popups.PopupAction));

        var area = GetNode<Area2D>(nameof(Area2D));

        area.MouseEntered += OnAreaMouseEntered;
        area.MouseExited += OnAreaMouseExited;
    }
}
