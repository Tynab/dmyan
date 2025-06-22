using Godot;
using System.Threading.Tasks;
using static DMYAN.Scripts.Constant;
using static Godot.AnimationMixer.SignalName;

namespace DMYAN.Scripts;

public partial class Card : Node2D
{
    public DuelSide DuelSide { get; set; } = DuelSide.None;

    public CardStatus Status { get; set; } = CardStatus.None;

    public CardZone Zone { get; set; } = CardZone.None;

    public CardFace CardFace { get; set; } = CardFace.None;

    public string Code { get; set; }

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

    private AnimationPlayer _animationPlayer;
    private Sprite2D _cardFront;
    private Sprite2D _cardBack;
    private bool _isHovered = false;

    public override void _Ready()
    {
        _animationPlayer = GetNodeOrNull<AnimationPlayer>("AnimationPlayer");
        _cardFront = GetNodeOrNull<Sprite2D>(CARD_FRONT_NODE);
        _cardBack = GetNodeOrNull<Sprite2D>(CARD_BACK_NODE);
    }

    public async Task PlayFlipAnimationAsync()
    {
        if (_animationPlayer is null)
        {
            return;
        }

        _animationPlayer.Play(CARD_DRAW_FLIP_ANIMATION);
        _ = await ToSignal(_animationPlayer, AnimationFinished);
    }

    private void OnMouseEntered()
    {
        if (DuelSide is DuelSide.Player && Status is CardStatus.InHand)
        {
            _isHovered = true;

            GD.Print($"Mouse entered card: {Code}");

            var hoverPosition = BasePosition;
            hoverPosition.Y -= 10;
            _ = GetTree().CreateTween().TweenProperty(this, "position", hoverPosition, 0.1).SetTrans(Tween.TransitionType.Linear).SetEase(Tween.EaseType.InOut);
        }
    }

    private void OnMouseExited()
    {
        if (DuelSide is DuelSide.Player && Status is CardStatus.InHand && _isHovered)
        {
            _isHovered = false;

            GD.Print($"Mouse exited card: {Code}");

            _ = GetTree().CreateTween().TweenProperty(this, "position", BasePosition, 0.1).SetTrans(Tween.TransitionType.Linear).SetEase(Tween.EaseType.InOut);
        }
    }
}
