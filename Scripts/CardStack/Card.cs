using DMYAN.Scripts.Common;
using DMYAN.Scripts.Common.Enum;
using Godot;
using System.Threading.Tasks;
using static DMYAN.Scripts.Common.Constant;
using static Godot.AnimationMixer.SignalName;
using static Godot.ResourceLoader;
using static Godot.Tween.EaseType;
using static Godot.Tween.TransitionType;
using static Godot.Vector2;

namespace DMYAN.Scripts.CardStack;

internal partial class Card : Node2D
{
    internal void Init(CardData data, DuelSide side)
    {
        _cardFront.Texture = Load<Texture2D>(data.Code.GetCardAssetPathByCode());
        _cardFront.Hide();

        _cardBack.Show();

        BaseSide = side;
        DuelSide = side;
        Location = CardLocation.None;
        Zone = CardZone.None;
        CardFace = CardFace.None;
        CardPosition = CardPosition.None;
        Type = data.Type;
        Property = data.Property;
        Attribute = data.Attribute;
        Race = data.Race;
        SummonType = data.SummonType;
        BanlistStatus = data.BanlistStatus;
        EffectType = data.EffectType;
        MainDeckIndex = default;
        ExtraDeckIndex = default;
        HandIndex = default;
        MainIndex = default;
        STPIndex = default;
        GraveyardIndex = default;
        BanishedIndex = default;
        Code = data.Code;
        CardName = data.Name;
        Description = data.Description;
        Level = data.Level;
        BaseATK = data.ATK;
        BaseDEF = data.DEF;
        ATK = data.ATK;
        DEF = data.DEF;
        BasePosition = Zero;
        CanActivate = false;
        CanSummon = false;
        CanSet = false;
        CanAttack = false;
        CanDirectAttack = false;
        ActionType = CardActionType.None;
    }

    internal void MainDeckEntered(int index)
    {
        Scale = One;
        Position = Zero;
        ZIndex = index;
        MainDeckIndex = index;
        Location = CardLocation.InDeck;
        Zone = CardZone.MainDeck;
    }

    internal void MainDeckExited()
    {
        MainDeckIndex = default;
        Location = CardLocation.None;
        Zone = CardZone.None;
    }

    internal void HandEntered(int index)
    {
        ZIndex = index;
        HandIndex = index;
        Location = CardLocation.InHand;
    }

    internal void HandExited()
    {
        HandIndex = default;
        Location = CardLocation.None;
    }

    internal async Task Summon(MainCardSlot cardSlot)
    {
        Reparent(cardSlot);

        ZIndex = cardSlot.CardsInSlot + 1;
        BasePosition = CARD_IN_SLOT_POSITION;
        Location = CardLocation.InBoard;
        Zone = CardZone.Main;
        CardFace = CardFace.FaceUp;
        CardPosition = CardPosition.Attack;

        await AnimationSummon(cardSlot.GlobalPosition, SCALE_MAX);
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

    internal void CanSummonOrSetCheck()
    {
        var isValid = (Level < 5 || _gameManager.PlayerMainZone.CardsInZone > 1) && _gameManager.PlayerMainZone.CardsInZone < 5 && !_gameManager.HasSummoned;

        CanSummon = isValid;
        CanSet = isValid;
    }

    internal void CannotSummonOrSetCheck()
    {
        CanSummon = false;
        CanSet = false;
    }

    internal async Task CanAttackCheck(DuelSide currentSide)
    {
        CanAttack = DuelSide == currentSide && Location is CardLocation.InBoard && Zone is CardZone.Main && CardFace is CardFace.FaceUp && CardPosition is CardPosition.Attack;

        if (CanAttack)
        {
            await Sword.Show(OPACITY_MAX);
        }
    }

    internal async Task AnimationFlipUpAsync()
    {
        _animationPlayer.Play(CARD_FLIP_UP_ANIMATION);

        _ = await ToSignal(_animationPlayer, AnimationFinished);
    }

    internal void AnimationDraw(Vector2 position) => GetTree().CreateTween().SetTrans(Circ).SetEase(Out).TweenProperty(this, POSITION_NODE_PATH, position, DEFAULT_ANIMATION_SPEED);

    internal async Task AnimationSummon(Vector2 globalPosition, Vector2 scale)
    {
        if (_gameManager.CurrentTurnSide is DuelSide.Opponent)
        {
            await AnimationFlipUpAsync();
        }

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
