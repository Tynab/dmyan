using DMYAN.Scripts.MainCardSlotStack;
using System.Collections.Generic;
using static Godot.GD;

namespace DMYAN.Scripts.MainZoneStack;

internal partial class MainZone : DMYANNode2D
{
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
