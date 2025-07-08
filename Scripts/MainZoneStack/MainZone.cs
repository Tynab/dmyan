using DMYAN.Scripts.CardStack;
using System.Threading.Tasks;

namespace DMYAN.Scripts.MainZoneStack;

internal partial class MainZone : DMYANNode2D
{
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

    internal void DestroyCard() => CardsInZone--;
}
