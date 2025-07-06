using Godot;
using System.Threading.Tasks;
using static DMYAN.Scripts.Common.Constant;
using static Godot.Mathf;
using static Godot.Tween.EaseType;
using static Godot.Tween.TransitionType;
using static Godot.Vector2;
using static System.Threading.Tasks.Task;

namespace DMYAN.Scripts.SwordStack;

internal partial class Sword : Node2D
{
    internal async Task FadeIn()
    {
        Show();

        _ = await ToSignal(GetTree().CreateTween().SetTrans(Sine).SetEase(InOut).TweenProperty(this, OPACITY_NODE_PATH, OPACITY_MAX, DEFAULT_ANIMATION_SPEED), FINISHED_SIGNAL);
    }

    internal async Task FadeOut()
    {
        _ = await ToSignal(GetTree().CreateTween().SetTrans(Sine).SetEase(InOut).TweenProperty(this, OPACITY_NODE_PATH, OPACITY_MIN, DEFAULT_ANIMATION_SPEED), FINISHED_SIGNAL);

        Hide();
    }

    internal async Task AnimationAttack(Vector2 globalPosition)
    {
        _isSwordFollowingMouse = false;

        _sprite.LookAt(globalPosition);
        _sprite.Rotation += Pi / 2;

        var parent = GetParent().GetParent<Node2D>();
        var h = _sprite.Texture.GetHeight() * parent.Scale.Y / 2;
        var angleRadians = DegToRad(90 - _sprite.RotationDegrees);

        _ = GetTree().CreateTween().SetTrans(Circ).SetEase(Out).TweenProperty(this, GLOBAL_POSITION_NODE_PATH, new Vector2(globalPosition.X - (float)(Cos(angleRadians) * h), globalPosition.Y + (float)(Sin(angleRadians) * h)), DEFAULT_ANIMATION_SPEED);

        await Delay(ATTACK_DELAY);

        await FadeOut();

        Position = Zero;

        _sprite.Rotation = _originalSwordRotation;
    }
}
