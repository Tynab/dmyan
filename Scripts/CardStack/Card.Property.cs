using DMYAN.Scripts.Common.Enum;
using DMYAN.Scripts.Popups;
using Godot;

namespace DMYAN.Scripts.CardStack;

internal partial class Card : Node2D
{
    internal DuelSide BaseSide { get; set; } = DuelSide.None;

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

    internal int? MainDeckIndex { get; set; } = null;

    internal int? ExtraDeckIndex { get; set; } = null;

    internal int? HandIndex { get; set; } = null;

    internal int? MainIndex { get; set; } = null;

    internal int? STPIndex { get; set; } = null;

    internal int? GraveyardIndex { get; set; } = null;

    internal int? BanishedIndex { get; set; } = null;

    internal int Id { get; set; }

    internal string Code { get; set; }

    internal string CardName { get; set; }

    internal string Description { get; set; } = string.Empty;

    internal int Level { get; set; } = 0;

    internal int BaseATK { get; set; } = 0;

    internal int BaseDEF { get; set; } = 0;

    internal int ATK { get; set; } = 0;

    internal int DEF { get; set; } = 0;

    internal Vector2 BasePosition { get; set; }

    internal bool CanActivate { get; set; } = false;

    internal bool CanSummon { get; set; } = false;

    internal bool CanSet { get; set; } = false;

    internal bool CanAttack { get; set; } = false;

    internal bool CanDirectAttack { get; set; } = false;

    internal CardActionType ActionType { get; set; } = CardActionType.None;

    internal Sword Sword { get; set; }

    internal PopupAction PopupAction { get; set; }
}
