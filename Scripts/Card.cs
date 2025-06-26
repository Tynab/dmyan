using DMYAN.Scripts.Common;
using DMYAN.Scripts.Popups;
using Godot;
using System.Threading.Tasks;
using static DMYAN.Scripts.Common.Constant;
using static Godot.AnimationMixer.SignalName;
using static Godot.Tween.EaseType;
using static Godot.Tween.TransitionType;

namespace DMYAN.Scripts;

internal partial class Card : Node2D
{
    internal DuelSide DuelSide { get; set; } = DuelSide.None;

    internal CardLocation Location { get; set; } = CardLocation.None;

    internal CardZone Zone { get; set; } = CardZone.None;

    internal CardFace CardFace { get; set; } = CardFace.None;

    internal CardPosition CardPosition { get; set; } = CardPosition.None;

    internal CardType Type { get; set; } = CardType.None;

    internal CardProperty Property { get; set; } = CardProperty.None;

    internal MonsterAttribute Attribute { get; set; } = MonsterAttribute.None;

    internal MonsterRace Race { get; set; } = MonsterRace.None;

    internal MonsterSummonType SummonType { get; set; } = MonsterSummonType.None;

    internal CardBanlistStatus BanlistStatus { get; set; } = CardBanlistStatus.None;

    internal CardEffectType EffectType { get; set; } = CardEffectType.None;

    internal string Code { get; set; }

    internal string CardName { get; set; }

    internal string Description { get; set; } = string.Empty;

    internal int Level { get; set; } = 0;

    internal int BaseATK { get; set; } = 0;

    internal int BaseDEF { get; set; } = 0;

    internal int ATK { get; set; } = 0;

    internal int DEF { get; set; } = 0;

    internal Vector2 BasePosition { get; set; }

    internal bool CanSummon { get; set; } = false;

    internal bool CanSet { get; set; } = false;

    internal bool CanActivate { get; set; } = false;

    internal bool CanAttack { get; set; } = false;

    internal bool CanDirectAttack { get; set; } = false;

    internal CardActionType ActionType { get; set; } = CardActionType.None;

    internal Sword Sword { get; set; }

    internal PopupAction PopupAction { get; set; }

    private Sprite2D _cardFront;
    private Sprite2D _cardBack;
    private AnimationPlayer _animationPlayer;

    private GameManager _gameManager;
    private MainZone _mainZone;
    private CardInfo _cardInfo;

    private bool _canView = false;

    public override void _Ready()
    {
        var field = GetParent().GetParent();

        _mainZone = field.GetNode<MainZone>(nameof(MainZone));

        var parent = field.GetParent().GetParent();

        _cardInfo = parent.GetNode<CardInfo>(nameof(CardInfo));
        _gameManager = parent.GetNode<GameManager>(nameof(GameManager));

        Sword = GetNode<Sword>(nameof(Sword));
        PopupAction = GetNode<PopupAction>(nameof(Popups.PopupAction));

        _cardFront = GetNode<Sprite2D>(CARD_FRONT_NODE);
        _cardBack = GetNode<Sprite2D>(CARD_BACK_NODE);
        _animationPlayer = GetNode<AnimationPlayer>(DEFAULT_ANIMATION_PLAYER_NODE);

        var area = GetNode<Area2D>(DEFAULT_AREA2D_NODE);

        area.MouseEntered += OnAreaMouseEntered;
        area.MouseExited += OnAreaMouseExited;
    }

    private void OnAreaMouseEntered()
    {
        if (DuelSide is DuelSide.Player)
        {
            if (Location is CardLocation.InBoard && Zone is CardZone.Field or CardZone.Main or CardZone.STP)
            {
                _canView = true;
                _cardInfo.BindingData(this);
            }
            else if (Location is CardLocation.InHand)
            {
                _canView = true;
                _cardInfo.BindingData(this);

                HighlightOn();

                if (CanActivate)
                {
                    PopupAction.ShowAction(PopupActionType.Activate);
                    ActionType = CardActionType.Activate;
                }
                else if ((Level < 5 || _mainZone.CardsInZone > 1) && _mainZone.CardsInZone < 5 && _gameManager.CurrentPhase is DuelPhase.Main1 or DuelPhase.Main2 && !_gameManager.HasSummoned)
                {
                    CanSummon = true;
                    CanSet = true;
                    PopupAction.ShowAction(PopupActionType.Summon);
                    ActionType = CardActionType.Summon;
                }
                else
                {
                    CanSummon = false;
                    CanSet = false;
                }
            }
        }
    }

    private void OnAreaMouseExited()
    {
        if (_canView)
        {
            if (Location is CardLocation.InHand)
            {
                HighlightOff();
            }

            PopupAction.Hide();
        }
    }

    internal void Summon(MainCardSlot cardSlot)
    {
        Reparent(cardSlot);

        BasePosition = CARD_IN_SLOT_POSITION;
        Location = CardLocation.InBoard;
        Zone = CardZone.Main;
        CardFace = CardFace.FaceUp;
        CardPosition = CardPosition.Attack;
        CanSummon = false;
        CanSet = false;

        AnimationSummon(cardSlot.GlobalPosition, CARD_IN_SLOT_SCALE);
    }

    internal void SummonSet(MainCardSlot cardSlot)
    {
        Reparent(cardSlot);

        BasePosition = CARD_IN_SLOT_POSITION;
        Location = CardLocation.InBoard;
        Zone = CardZone.Main;
        CardFace = CardFace.FaceDown;
        CardPosition = CardPosition.Defense;
        CanSummon = false;
        CanSet = false;

        AnimationSummonSet(cardSlot.GlobalPosition, CARD_IN_SLOT_SCALE);
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

    internal async Task CanAttackCheck(DuelSide currentSide)
    {
        CanAttack = DuelSide == currentSide && Location is CardLocation.InBoard && Zone is CardZone.Main && CardFace is CardFace.FaceUp && CardPosition is CardPosition.Attack;

        if (CanAttack)
        {
            await Sword.FadeIn(OPACITY_MAX);
        }
    }

    private void HighlightOn()
    {
        var newPosition = BasePosition;

        newPosition.Y -= CARD_HAND_RAISE_Y;

        _ = GetTree().CreateTween().SetTrans(Linear).SetEase(InOut).TweenProperty(this, POSITION_NODE_PATH, newPosition, DEFAULT_ANIMATION_SPEED);
    }

    private void HighlightOff() => GetTree().CreateTween().SetTrans(Linear).SetEase(InOut).TweenProperty(this, POSITION_NODE_PATH, BasePosition, DEFAULT_ANIMATION_SPEED);

    private void ResetPower()
    {
        ATK = BaseATK;
        DEF = BaseDEF;
    }
}
