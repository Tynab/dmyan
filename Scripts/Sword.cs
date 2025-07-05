using DMYAN.Scripts.GameManagerStack;
using Godot;
using System.Threading.Tasks;
using static DMYAN.Scripts.Common.Constant;
using static Godot.Mathf;
using static Godot.MouseButton;
using static Godot.Tween.EaseType;
using static Godot.Tween.TransitionType;

namespace DMYAN.Scripts;

internal partial class Sword : Node2D
{
    private GameManager _gameManager;
    private Sprite2D _sprite;

    private bool _isSwordFollowingMouse = false;
    private float _originalSwordRotation;

    public override void _Ready()
    {
        var main = GetTree().Root.GetNode(MAIN_NODE);

        _gameManager = main.GetNode<GameManager>(nameof(GameManager));

        _sprite = GetNode<Sprite2D>(DEFAULT_SPRITE2D_NODE);

        _originalSwordRotation = _sprite.Rotation;

        var area = GetNode<Area2D>(DEFAULT_AREA2D_NODE);

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
            _gameManager.AttackMode = false;
            _isSwordFollowingMouse = false;
            _sprite.Rotation = _originalSwordRotation;
        }
    }

    private void OnAreaInputEvent(Node viewport, InputEvent @event, long shapeIdx)
    {
        if (@event is InputEventMouseButton mouseButtonEvent && mouseButtonEvent.Pressed && mouseButtonEvent.ButtonIndex is Left)
        {
            if (_gameManager.AttackMode)
            {
                return;
            }
            else
            {
                _gameManager.AttackMode = true;
            }

            var opposite = GameManager.GetOpposite(_gameManager.CurrentTurnSide);

            if (_gameManager.HasCardInMainZone(opposite))
            {
                _isSwordFollowingMouse = true;
            }
            else
            {
                AnimationAttack(_gameManager.GetAvatarPosition(opposite));
            }
        }
    }

    internal async Task Show(int opacity)
    {
        Show();

        _ = await ToSignal(GetTree().CreateTween().SetTrans(Sine).SetEase(InOut).TweenProperty(this, OPACITY_NODE_PATH, opacity, DEFAULT_ANIMATION_SPEED), FINISHED_SIGNAL);
    }

    internal async Task Hide(int opacity)
    {
        _ = await ToSignal(GetTree().CreateTween().SetTrans(Sine).SetEase(InOut).TweenProperty(this, OPACITY_NODE_PATH, opacity, DEFAULT_ANIMATION_SPEED), FINISHED_SIGNAL);

        Hide();
    }

    private void AnimationAttack(Vector2 position)
    {
        _sprite.LookAt(position);
        _sprite.Rotation += Pi / 2;

        var parent = GetParent().GetParent<Node2D>();
        var h = _sprite.Texture.GetHeight() * parent.Scale.Y / 2;
        var angleRadians = DegToRad(90 - _sprite.RotationDegrees);
        var x = Cos(angleRadians) * h;
        var y = Sin(angleRadians) * h;

        _ = GetTree().CreateTween().SetTrans(Circ).SetEase(Out).TweenProperty(this, GLOBAL_POSITION_NODE_PATH, new Vector2(position.X - (float)x, position.Y + (float)y), DEFAULT_ANIMATION_SPEED);
    }
}
