using DMYAN.Scripts.Common.Enum;
using Godot;
using System.Threading.Tasks;
using static DMYAN.Scripts.Common.Constant;
using static Godot.ResourceLoader;
using static Godot.Tween.EaseType;
using static Godot.Tween.TransitionType;
using static System.Threading.Tasks.Task;

namespace DMYAN.Scripts.Popups;

internal partial class PopupPhase : Sprite2D
{
    internal async Task ShowPhase(DuelSide side, DuelPhase phase)
    {
        var path = side switch
        {
            DuelSide.Player => phase switch
            {
                DuelPhase.Draw => PLAYER_DP_POPUP_ASSET_PATH,
                DuelPhase.Standby => PLAYER_SP_POPUP_ASSET_PATH,
                DuelPhase.Main1 => PLAYER_M1_POPUP_ASSET_PATH,
                DuelPhase.Battle => PLAYER_BP_POPUP_ASSET_PATH,
                DuelPhase.Main2 => PLAYER_M2_POPUP_ASSET_PATH,
                DuelPhase.End => PLAYER_EP_POPUP_ASSET_PATH,
                _ => string.Empty
            },
            DuelSide.Opponent => phase switch
            {
                DuelPhase.Draw => OPPONENT_DP_POPUP_ASSET_PATH,
                DuelPhase.Standby => OPPONENT_SP_POPUP_ASSET_PATH,
                DuelPhase.Main1 => OPPONENT_M1_POPUP_ASSET_PATH,
                DuelPhase.Battle => OPPONENT_BP_POPUP_ASSET_PATH,
                DuelPhase.Main2 => OPPONENT_M2_POPUP_ASSET_PATH,
                DuelPhase.End => OPPONENT_EP_POPUP_ASSET_PATH,
                _ => string.Empty
            },
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
