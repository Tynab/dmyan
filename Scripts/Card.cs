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

    private AnimationPlayer _animationPlayer;
    private Sprite2D _cardFront;
    private Sprite2D _cardBack;
    private MainZone _mainZone;
    private CardInfo _cardInfo;
    private PopupAction _popupAction;
    private bool _canView = false;

    public override void _Ready()
    {
        _animationPlayer = GetNodeOrNull<AnimationPlayer>(DEFAULT_ANIMATION_PLAYER_NODE);
        _cardFront = GetNodeOrNull<Sprite2D>(CARD_FRONT_NODE);
        _cardBack = GetNodeOrNull<Sprite2D>(CARD_BACK_NODE);
        _mainZone = GetNodeOrNull<MainZone>($"../../{nameof(MainZone)}");
        _cardInfo = GetNodeOrNull<CardInfo>($"../../../../{nameof(CardInfo)}");
        _popupAction = GetNodeOrNull<PopupAction>(nameof(PopupAction));
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

    private void OnMouseEntered()
    {
        if (DuelSide is DuelSide.Player)
        {
            if (Status is CardStatus.InBoard && Zone is CardZone.Field or CardZone.Main or CardZone.STP)
            {
                BindingDataToCardInfo();
            }
            else if (Status is CardStatus.InHand)
            {
                HighlightOn();
                BindingDataToCardInfo();

                if ((Level < 5 || _mainZone.CardsInZone > 1) && _mainZone.CardsInZone < 5)
                {
                    CanSummon = true;
                    _popupAction.ShowSummonPopup();
                }
                else
                {
                    CanSummon = false;
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

            _popupAction.HidePopup();
        }
    }

    private void HighlightOn()
    {
        var hoverPosition = BasePosition;

        hoverPosition.Y -= CARD_HAND_RAISE_Y;
        _ = GetTree().CreateTween().SetTrans(Linear).SetEase(InOut).TweenProperty(this, POSITION_NODE_PATH, hoverPosition, DEFAULT_ANIMATION_SPEED);
    }

    private void HighlightOff() => GetTree().CreateTween().SetTrans(Linear).SetEase(InOut).TweenProperty(this, POSITION_NODE_PATH, BasePosition, DEFAULT_ANIMATION_SPEED);

    private void BindingDataToCardInfo()
    {
        _canView = true;

        if (_cardInfo.CurrentSwap is DEFAULT_CARD_INFO_SWAP)
        {
            _cardInfo.TexturePath2 = Code.GetCardAssetPathByCode();
        }
        else
        {
            _cardInfo.TexturePath1 = Code.GetCardAssetPathByCode();
        }

        _cardInfo.UpdateTexture();
        _cardInfo.UpdateDescription(CardName, Type, Property, Attribute, Race, Level, ATK, DEF, Description);
    }
}
