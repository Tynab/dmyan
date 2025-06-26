using Godot;
using System;
using System.Drawing;
using System.Threading.Tasks;
using static DMYAN.Scripts.Common.Constant;
using static Godot.Mathf;
using static Godot.MouseButton;
using static Godot.Tween.EaseType;
using static Godot.Tween.TransitionType;

namespace DMYAN.Scripts;

public partial class Eye : Node2D
{
    internal async Task Show(int opacity, Vector2 scale)
    {
        Show();

        _ = await ToSignal(GetTree().CreateTween().SetTrans(Sine).SetEase(InOut).TweenProperty(this, OPACITY_NODE_PATH, opacity, 1), FINISHED_SIGNAL);
        _ = await ToSignal(GetTree().CreateTween().SetTrans(Sine).SetEase(Out).TweenProperty(this, SCALE_NODE_PATH, scale, 1), FINISHED_SIGNAL);
    }

    internal async Task Hide(int opacity, Vector2 scale)
    {
        _ = await ToSignal(GetTree().CreateTween().SetTrans(Sine).SetEase(InOut).TweenProperty(this, OPACITY_NODE_PATH, opacity, 1), FINISHED_SIGNAL);
        _ = await ToSignal(GetTree().CreateTween().SetTrans(Sine).SetEase(Out).TweenProperty(this, SCALE_NODE_PATH, scale, 1), FINISHED_SIGNAL);

        Hide();
    }
}
