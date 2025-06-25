using DMYAN.Scripts.Popups;
using Godot;
using System.Threading.Tasks;
using static DMYAN.Scripts.Constant;
using static Godot.AnimationMixer.SignalName;
using static Godot.Tween.EaseType;
using static Godot.Tween.TransitionType;

namespace DMYAN.Scripts;

public partial class Card : Node2D
{
    public DuelSide DuelSide { get; set; } = DuelSide.None;

    public CardStatus Status { get; set; } = CardStatus.None;

    public CardZone Zone { get; set; } = CardZone.None;

    public CardFace CardFace { get; set; } = CardFace.None;

    public string Code { get; set; }

    public string CardName { get; set; }

    public string Description { get; set; } = string.Empty;

    public CardType Type { get; set; } = CardType.None;

    public CardProperty Property { get; set; } = CardProperty.None;

    public MonsterAttribute Attribute { get; set; } = MonsterAttribute.None;

    public MonsterRace Race { get; set; } = MonsterRace.None;

    public MonsterSummonType SummonType { get; set; } = MonsterSummonType.None;

    public int Level { get; set; } = 0;

    public int ATK { get; set; } = 0;

    public int DEF { get; set; } = 0;

    public CardBanlistStatus BanlistStatus { get; set; } = CardBanlistStatus.None;

    public CardEffectType EffectType { get; set; } = CardEffectType.None;

    public Vector2 BasePosition { get; set; }

    public bool CanSummon { get; set; } = false;

    public bool CanSet { get; set; } = false;

    public bool CanActivate { get; set; } = false;

    public CardActionType ActionType { get; set; } = CardActionType.None;

    public bool CanAttack { get; set; } = false;

    public PopupAction PopupAction { get; set; }

    private AnimationPlayer _animationPlayer;
    private Sprite2D _cardFront;
    private Sprite2D _cardBack;
    private GameManager _gameManager;
    private MainZone _mainZone;
    private CardInfo _cardInfo;
    private bool _canView = false;

    public override void _Ready()
    {
        _animationPlayer = GetNode<AnimationPlayer>(DEFAULT_ANIMATION_PLAYER_NODE);
        _cardFront = GetNode<Sprite2D>(CARD_FRONT_NODE);
        _cardBack = GetNode<Sprite2D>(CARD_BACK_NODE);
        _gameManager = GetNode<GameManager>($"../../../../{nameof(GameManager)}");
        _mainZone = GetNode<MainZone>($"../../{nameof(MainZone)}");
        _cardInfo = GetNode<CardInfo>($"../../../../{nameof(CardInfo)}");
        PopupAction = GetNode<PopupAction>(nameof(Popups.PopupAction));
    }

    public void Summon(MainCardSlot cardSlot)
    {
        Reparent(cardSlot);
        BasePosition = CARD_IN_SLOT_POSITION;
        Status = CardStatus.InBoard;
        Zone = CardZone.Main;
        CardFace = CardFace.FaceUp;
        CanSummon = false;
        CanSet = false;
        AnimationSummon(cardSlot.GlobalPosition, CARD_IN_SLOT_SCALE);
    }

    public void SummonSet(MainCardSlot cardSlot)
    {
        Reparent(cardSlot);
        BasePosition = CARD_IN_SLOT_POSITION;
        Status = CardStatus.InBoard;
        Zone = CardZone.Main;
        CardFace = CardFace.FaceDown;
        CanSummon = false;
        CanSet = false;
        AnimationSummonSet(cardSlot.GlobalPosition, CARD_IN_SLOT_SCALE);
    }

    public async Task AnimationDrawFlipAsync()
    {
        _animationPlayer.Play(CARD_DRAW_FLIP_ANIMATION);
        _ = await ToSignal(_animationPlayer, AnimationFinished);
    }

    public void AnimationDraw(Vector2 position) => GetTree().CreateTween().SetTrans(Circ).SetEase(Out).TweenProperty(this, POSITION_NODE_PATH, position, DEFAULT_ANIMATION_SPEED);

    public void AnimationSummon(Vector2 globalPosition, Vector2 scale)
    {
        _ = GetTree().CreateTween().SetTrans(Sine).SetEase(Out).TweenProperty(this, GLOBAL_POSITION_NODE_PATH, globalPosition, DEFAULT_ANIMATION_SPEED);
        _ = GetTree().CreateTween().SetTrans(Sine).SetEase(Out).TweenProperty(this, SCALE_NODE_PATH, scale, DEFAULT_ANIMATION_SPEED);
    }

    public void AnimationSet(Vector2 globalPosition, Vector2 scale)
    {
        _animationPlayer.Play(CARD_FLIP_DOWN_ANIMATION);
        _ = GetTree().CreateTween().SetTrans(Sine).SetEase(Out).TweenProperty(this, GLOBAL_POSITION_NODE_PATH, globalPosition, DEFAULT_ANIMATION_SPEED);
        _ = GetTree().CreateTween().SetTrans(Sine).SetEase(Out).TweenProperty(this, SCALE_NODE_PATH, scale, DEFAULT_ANIMATION_SPEED);
    }

    public void AnimationSummonSet(Vector2 globalPosition, Vector2 scale)
    {
        AnimationSet(globalPosition, scale);
        _ = GetTree().CreateTween().SetTrans(Sine).SetEase(Out).TweenProperty(this, ROTATION_NODE_PATH, RotationDegrees - 90, DEFAULT_ANIMATION_SPEED);
    }

    private void OnMouseEntered()
    {
        if (DuelSide is DuelSide.Player)
        {
            if (Status is CardStatus.InBoard && Zone is CardZone.Field or CardZone.Main or CardZone.STP)
            {
                _canView = true;
                _cardInfo.BindingData(this);
            }
            else if (Status is CardStatus.InHand)
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

    private void OnMouseExited()
    {
        if (_canView)
        {
            if (Status is CardStatus.InHand)
            {
                HighlightOff();
            }

            PopupAction.Hide();
        }
    }

    private void HighlightOn()
    {
        var newPosition = BasePosition;

        newPosition.Y -= CARD_HAND_RAISE_Y;
        _ = GetTree().CreateTween().SetTrans(Linear).SetEase(InOut).TweenProperty(this, POSITION_NODE_PATH, newPosition, DEFAULT_ANIMATION_SPEED);
    }

    private void HighlightOff() => GetTree().CreateTween().SetTrans(Linear).SetEase(InOut).TweenProperty(this, POSITION_NODE_PATH, BasePosition, DEFAULT_ANIMATION_SPEED);
}
