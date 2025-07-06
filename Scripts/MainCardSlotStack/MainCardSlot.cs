using DMYAN.Scripts.CardStack;
using DMYAN.Scripts.Common.Enum;
using DMYAN.Scripts.MainZoneStack;
using System.Threading.Tasks;

namespace DMYAN.Scripts.MainCardSlotStack;

internal partial class MainCardSlot : CardSlot
{
    internal async Task SummonCard(Card card)
    {
        await card.Summon(this);

        HasCardInSlot = true;

        PowerSlot.ShowPower(card, true);
    }

    internal void SummonSetCard(Card card)
    {
        card.SummonSet(this);

        HasCardInSlot = true;

        if (DuelSide is DuelSide.Player)
        {
            PowerSlot.ShowPower(card, false);
        }
    }

    internal void DestroyCard()
    {
        GetParent<MainZone>().DestroyCard();

        HasCardInSlot = false;

        PowerSlot.HidePower();
    }
}
