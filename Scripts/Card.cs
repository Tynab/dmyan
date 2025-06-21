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

    private AnimationPlayer _animationPlayer;
    private Sprite2D _cardFront;
    private Sprite2D _cardBack;

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
}
