using Godot;
using System.Collections.Generic;
using static Godot.GD;

namespace DMYAN.Scripts;

public partial class MainZone : Node2D
{
    [Export]
    public DuelSide DuelSide { get; set; } = DuelSide.None;

    public int CardsInZone { get; set; } = 0;

    public bool HasCardCanAttack { get; set; } = false;

    public void SummonCard(Card card)
    {
        GetMainSlot(true).SummonCard(card);
        CardsInZone++;
    }

    public void SummonSetCard(Card card)
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
