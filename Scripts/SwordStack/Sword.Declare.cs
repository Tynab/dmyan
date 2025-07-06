using DMYAN.Scripts.GameManagerStack;
using Godot;

namespace DMYAN.Scripts.SwordStack;

internal partial class Sword : Node2D
{
    private GameManager _gameManager;

    private Sprite2D _sprite;

    private float _originalSwordRotation;
    private bool _isSwordFollowingMouse = false;
}
