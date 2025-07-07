using Godot;
using static DMYAN.Scripts.Common.Constant;
using static Godot.Tween.EaseType;
using static Godot.Tween.TransitionType;

namespace DMYAN.Scripts.CardStack;

internal partial class Card : Node2D
{
    private void HighlightOn()
    {
        var newPosition = BasePosition;

        newPosition.Y -= CARD_HAND_RAISE_Y;

        _ = GetTree().CreateTween().SetTrans(Linear).SetEase(InOut).TweenProperty(this, POSITION_NODE_PATH, newPosition, DEFAULT_ANIMATION_SPEED);
    }

    private void HighlightOff() => GetTree().CreateTween().SetTrans(Linear).SetEase(InOut).TweenProperty(this, POSITION_NODE_PATH, BasePosition, DEFAULT_ANIMATION_SPEED);

    private void ResetDefault()
    {
        RotationDegrees = 0;

        ResetFace();
        ResetPower();
    }

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
}
