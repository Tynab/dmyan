using DMYAN.Scripts.GameManagerStack;
using Godot;
using static DMYAN.Scripts.Common.Constant;
using static Godot.Mathf;
using static Godot.MouseButton;

namespace DMYAN.Scripts.SwordStack;

internal partial class Sword : DMYANNode2D
{
    public override void _Ready()
    {
        var main = GetTree().Root.GetNode(MAIN_NODE);

        _gameManager = main.GetNode<GameManager>(nameof(GameManager));

        _sprite = GetNode<Sprite2D>(nameof(Sprite2D));

        _originalSwordRotation = _sprite.Rotation;

        var area = GetNode<Area2D>(nameof(Area2D));

        area.InputEvent += OnAreaInputEvent;
    }

    public override void _Process(double delta)
    {
        if (_isSwordFollowingMouse)
        {
            _sprite.LookAt(GetGlobalMousePosition());
            _sprite.Rotation += Pi / 2;
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (_isSwordFollowingMouse && @event is InputEventMouseButton mouseEvent && mouseEvent.Pressed && mouseEvent.ButtonIndex is Right)
        {
            _isSwordFollowingMouse = false;
            _sprite.Rotation = _originalSwordRotation;
        }
    }
}
