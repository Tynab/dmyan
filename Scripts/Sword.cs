using Godot;
using System;
using static Godot.Mathf;
using static Godot.MouseButton;
using static DMYAN.Scripts.Common.Constant;

namespace DMYAN.Scripts;

public partial class Sword : Node2D
{
    private Sprite2D _swordSprite;
    private Area2D _swordArea;

    private bool _isSwordFollowingMouse = false;
    private float _originalSwordRotation;

    public override void _Ready()
    {
        _swordSprite = GetNode<Sprite2D>(SPRITE2D_PLAYER_NODE);
        _swordArea = GetNode<Area2D>(AREA2D_PLAYER_NODE);

        _originalSwordRotation = _swordSprite.Rotation;

        _swordArea.InputEvent += OnAreaInputEvent;
    }

    public override void _Process(double delta)
    {
        if (_isSwordFollowingMouse)
        {
            _swordSprite.LookAt(GetGlobalMousePosition());
            _swordSprite.Rotation += Pi / 2;
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (_isSwordFollowingMouse && @event is InputEventMouseButton mouseEvent && mouseEvent.Pressed && mouseEvent.ButtonIndex is Right)
        {
            _isSwordFollowingMouse = false;
            _swordSprite.Rotation = _originalSwordRotation;
        }
    }

    private void OnAreaInputEvent(Node viewport, InputEvent @event, long shapeIdx)
    {
        if (@event is InputEventMouseButton mouseButtonEvent && mouseButtonEvent.Pressed && mouseButtonEvent.ButtonIndex is Left)
        {
            _isSwordFollowingMouse = true;
        }
    }
}
