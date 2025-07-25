using DMYAN.Scripts.GameManagerStack;
using Godot;

namespace DMYAN.Scripts.CardStack;

internal partial class Card : DMYANNode2D
{
    private GameManager _gameManager;

    private Sprite2D _cardFront;
    private Sprite2D _cardBack;
    private AnimationPlayer _animationPlayer;
}
