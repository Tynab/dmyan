using DMYAN.Scripts.Common.Enum;
using Godot;

namespace DMYAN.Scripts;

internal partial class MainCardSlot : CardSlot
{
    [Export]
    private PowerSlot PowerSlot { get; set; }

    internal int CardsInSlot { get; set; } = 0;

    internal bool HasCardCanAttack { get; set; } = false;

    internal void SummonCard(Card card)
    {
        card.Summon(this);

        HasCardInSlot = true;
        CardsInSlot++;

        PowerSlot.ShowPower(card, true);
    }

    internal void SummonSetCard(Card card)
    {
        card.SummonSet(this);

        HasCardInSlot = true;
        CardsInSlot++;

        if (DuelSide is DuelSide.Player)
        {
            PowerSlot.ShowPower(card, false);
        }
    }
}
