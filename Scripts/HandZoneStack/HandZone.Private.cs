using DMYAN.Scripts.CardStack;
using DMYAN.Scripts.Common.Enum;
using System.Threading.Tasks;
using static Godot.Vector2;

namespace DMYAN.Scripts.HandZoneStack;

internal partial class HandZone : DMYANNode2D
{
    internal async Task AddCard(Card card)
    {
        card.Reparent(this);

        card.Scale = One;
        card.Location = CardLocation.InHand;
        card.HandIndex = GetChildCount();

        ArrangeCardsAndResetIndex();

        if (DuelSide is DuelSide.Player)
        {
            await card.AnimationFlipUp();

            card.CanView = true;
        }
    }

    internal void RemoveCard(Card card)
    {
        card.HandExited();

        ArrangeCardsAndResetIndex();
    }
}
