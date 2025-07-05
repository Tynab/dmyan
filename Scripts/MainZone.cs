using DMYAN.Scripts.CardStack;
using DMYAN.Scripts.Common.Enum;
using Godot;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Godot.GD;

namespace DMYAN.Scripts;

internal partial class MainZone : Node2D
{
    [Export]
    private DuelSide DuelSide { get; set; } = DuelSide.None;

    internal int CardsInZone { get; set; } = 0;

    internal bool HasCardCanAttack { get; set; } = false;

    internal async Task SummonCard(Card card)
    {
        await GetMainSlot(true).SummonCard(card);

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

        for (var i = 0; i < 5; i++)
        {
            if (GetChild(i) is MainCardSlot vSlot && !vSlot.HasCardInSlot && GetChild(i + 5) is MainCardSlot hSlot && !hSlot.HasCardInSlot)
            {
                if (isAtk)
                {
                    emptySlots.Add(vSlot);
                }
                else
                {
                    emptySlots.Add(hSlot);
                }
            }
        }

        return emptySlots.Count > 0 ? emptySlots[RandRange(0, emptySlots.Count - 1)] : default;
    }
}
