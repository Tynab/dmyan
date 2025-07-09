using static DMYAN.Scripts.Common.Constant;
using static Godot.Tween.EaseType;
using static Godot.Tween.TransitionType;

namespace DMYAN.Scripts.CardStack;

internal partial class Card : DMYANNode2D
{
    #region Animation

    private void AnimationHighlightOn()
    {
        var position = BasePosition;

        position.Y -= CARD_HAND_RAISE_Y;

        _ = GetTree().CreateTween().SetTrans(Linear).SetEase(InOut).TweenProperty(this, POSITION_NODE_PATH, position, DEFAULT_ANIMATION_SPEED);
    }

    private void AnimationHighlightOff() => GetTree().CreateTween().SetTrans(Linear).SetEase(InOut).TweenProperty(this, POSITION_NODE_PATH, BasePosition, DEFAULT_ANIMATION_SPEED);

    #endregion

    #region Reset State

    private void ResetDefault()
    {
        ResetRotation();
        ResetFace();
        ResetPower();
    }

    private void ResetRotation() => RotationDegrees = 0;

    private void ResetFace()
    {
        _cardFront.Hide();
        _cardBack.Show();
    }

    private void ResetPower()
    {
        ATK = BaseATK;
        DEF = BaseDEF;
    }

    #endregion
}
