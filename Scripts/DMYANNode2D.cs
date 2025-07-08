using Godot;
using System.Threading.Tasks;
using static DMYAN.Scripts.Common.Constant;
using static Godot.Tween.EaseType;
using static Godot.Tween.TransitionType;

namespace DMYAN.Scripts;

internal partial class DMYANNode2D : Node2D
{
    internal async Task FadeIn(float speed = FADE_ANIMATION_SPEED)
    {
        Show();

        _ = await ToSignal(GetTree().CreateTween().SetTrans(Sine).SetEase(InOut).TweenProperty(this, OPACITY_NODE_PATH, OPACITY_MAX, speed), FINISHED_SIGNAL);
    }

    internal async Task FadeOut(float speed = FADE_ANIMATION_SPEED)
    {
        _ = await ToSignal(GetTree().CreateTween().SetTrans(Sine).SetEase(InOut).TweenProperty(this, OPACITY_NODE_PATH, OPACITY_MIN, speed), FINISHED_SIGNAL);

        Hide();
    }
}
