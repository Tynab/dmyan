using DMYAN.Scripts.Common;
using Godot;
using System.Collections.Generic;
using static Godot.GD;

namespace DMYAN.Scripts;

internal partial class MainZone : Node2D
{
    [Export]
    private DuelSide DuelSide { get; set; } = DuelSide.None;

    internal int CardsInZone { get; set; } = 0;

    internal bool HasCardCanAttack { get; set; } = false;

    internal void SummonCard(Card card)
    {
        GetMainSlot(true).SummonCard(card);

        CardsInZone++;
    }

    internal void SummonSetCard(Card card)
    {
        GetMainSlot(false).SummonSetCard(card);

        CardsInZone++;
    }

    private MainCardSlot GetMainSlot(bool isAtk)
    {
        var emptySlots = new List<MainCardSlot>();

        for (var i = 0; i < GetChildCount(); i++)
        {
            if (GetChild(i) is MainCardSlot currentSlot && !currentSlot.HasCardInSlot)
            {
                if (isAtk && i < 5)
                {
                    emptySlots.Add(currentSlot);
                }
                else if (i is > 4 and < 10)
                {
                    emptySlots.Add(currentSlot);
                }
            }
        }

        return emptySlots.Count > 0 ? emptySlots[RandRange(0, emptySlots.Count - 1)] : default;
    }
}
