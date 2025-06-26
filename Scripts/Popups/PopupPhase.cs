using DMYAN.Scripts.Common;
using Godot;
using System.Threading.Tasks;
using static DMYAN.Scripts.Common.Constant;
using static Godot.ResourceLoader;
using static Godot.Tween.EaseType;
using static Godot.Tween.TransitionType;
using static Godot.Vector2;
using static System.Threading.Tasks.Task;

namespace DMYAN.Scripts.Popups;

internal partial class PopupPhase : Sprite2D
{
    internal async Task ShowPhase(PopupPhaseType type)
    {
        var path = type switch
        {
            PopupPhaseType.DP => DP_POPUP_ASSET_PATH,
            PopupPhaseType.SP => SP_POPUP_ASSET_PATH,
            PopupPhaseType.M1 => M1_POPUP_ASSET_PATH,
            PopupPhaseType.BP => BP_POPUP_ASSET_PATH,
            PopupPhaseType.M2 => M2_POPUP_ASSET_PATH,
            PopupPhaseType.EP => EP_POPUP_ASSET_PATH,
            _ => string.Empty
        };

        if (string.IsNullOrWhiteSpace(path))
        {
            return;
        }

        Texture = Load<Texture2D>(path);

        Show();

        _ = await ToSignal(GetTree().CreateTween().SetTrans(Sine).SetEase(Out).TweenProperty(this, SCALE_NODE_PATH, SCALE_MAX, PHASE_ANIMATION_SPEED), FINISHED_SIGNAL);

        await Delay(PHASE_CHANGE_DELAY);

        _ = await ToSignal(GetTree().CreateTween().SetTrans(Sine).SetEase(Out).TweenProperty(this, SCALE_NODE_PATH, SCALE_MIN, PHASE_ANIMATION_SPEED), FINISHED_SIGNAL);

        Hide();
    }
}
