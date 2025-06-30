using DMYAN.Scripts.GameManagerStack;
using Godot;

namespace DMYAN.Scripts.CardStack;

internal partial class Card : Node2D
{
    private Sprite2D _cardFront;
    private Sprite2D _cardBack;
    private AnimationPlayer _animationPlayer;

    private GameManager _gameManager;
    private MainZone _mainZone;
    private CardInfo _cardInfo;

    private bool _canView = false;
}
