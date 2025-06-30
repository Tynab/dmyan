using DMYAN.Scripts.Common.Enum;
using Godot;
using System.Threading.Tasks;
using static DMYAN.Scripts.Common.Constant;
using static Godot.AnimationMixer.SignalName;
using static Godot.Tween.EaseType;
using static Godot.Tween.TransitionType;

namespace DMYAN.Scripts.CardStack;

internal partial class Card : Node2D
{
    internal void Summon(MainCardSlot cardSlot)
    {
        Reparent(cardSlot);

        BasePosition = CARD_IN_SLOT_POSITION;
        Location = CardLocation.InBoard;
        Zone = CardZone.Main;
        CardFace = CardFace.FaceUp;
        CardPosition = CardPosition.Attack;

        AnimationSummon(cardSlot.GlobalPosition, SCALE_MAX);
    }

    internal void SummonSet(MainCardSlot cardSlot)
    {
        Reparent(cardSlot);

        BasePosition = CARD_IN_SLOT_POSITION;
        Location = CardLocation.InBoard;
        Zone = CardZone.Main;
        CardFace = CardFace.FaceDown;
        CardPosition = CardPosition.Defense;

        AnimationSummonSet(cardSlot.GlobalPosition, SCALE_MAX);
    }

    internal void CanSummonCheck()
    {
        var isValid = (Level < 5 || _mainZone.CardsInZone > 1) && _mainZone.CardsInZone < 5 && !_gameManager.HasSummoned;

        CanSummon = isValid;
        CanSet = isValid;
    }

    internal async Task CanAttackCheck(DuelSide currentSide)
    {
        CanAttack = DuelSide == currentSide && Location is CardLocation.InBoard && Zone is CardZone.Main && CardFace is CardFace.FaceUp && CardPosition is CardPosition.Attack;

        if (CanAttack)
        {
            await Sword.Show(OPACITY_MAX);
        }
    }

    internal async Task AnimationDrawFlipAsync()
    {
        _animationPlayer.Play(CARD_DRAW_FLIP_ANIMATION);

        _ = await ToSignal(_animationPlayer, AnimationFinished);
    }

    internal void AnimationDraw(Vector2 position) => GetTree().CreateTween().SetTrans(Circ).SetEase(Out).TweenProperty(this, POSITION_NODE_PATH, position, DEFAULT_ANIMATION_SPEED);

    internal void AnimationSummon(Vector2 globalPosition, Vector2 scale)
    {
        _ = GetTree().CreateTween().SetTrans(Sine).SetEase(Out).TweenProperty(this, GLOBAL_POSITION_NODE_PATH, globalPosition, DEFAULT_ANIMATION_SPEED);
        _ = GetTree().CreateTween().SetTrans(Sine).SetEase(Out).TweenProperty(this, SCALE_NODE_PATH, scale, DEFAULT_ANIMATION_SPEED);
    }

    internal void AnimationSet(Vector2 globalPosition, Vector2 scale)
    {
        _animationPlayer.Play(CARD_FLIP_DOWN_ANIMATION);

        _ = GetTree().CreateTween().SetTrans(Sine).SetEase(Out).TweenProperty(this, GLOBAL_POSITION_NODE_PATH, globalPosition, DEFAULT_ANIMATION_SPEED);
        _ = GetTree().CreateTween().SetTrans(Sine).SetEase(Out).TweenProperty(this, SCALE_NODE_PATH, scale, DEFAULT_ANIMATION_SPEED);
    }

    internal void AnimationSummonSet(Vector2 globalPosition, Vector2 scale)
    {
        AnimationSet(globalPosition, scale);

        _ = GetTree().CreateTween().SetTrans(Sine).SetEase(Out).TweenProperty(this, ROTATION_NODE_PATH, RotationDegrees - 90, DEFAULT_ANIMATION_SPEED);
    }
}
