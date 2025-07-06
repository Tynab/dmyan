using DMYAN.Scripts.CardStack;
using DMYAN.Scripts.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using YANLib;

namespace DMYAN.Scripts.MainDeckStack;

internal partial class MainDeck : CardSlot
{
    internal void Init(List<Card> cards)
    {
        for (var i = 0; i < cards.Count; i++)
        {
            AddCard(cards[i], i);
        }

        ShuffleAndArrange();
    }

    internal void AddCard(Card card, int index)
    {
        card.Reparent(this);
        card.MainDeckEntered(index);

        UpdateCountDisplay();
    }

    internal Card RemoveCard()
    {
        var cards = _gameManager.GetCardsInMainDeck(DuelSide);

        if (cards.IsNullEmpty())
        {
            return default;
        }

        var card = cards.OrderByDescending(static x => x.MainDeckIndex).FirstOrDefault();

        card.Location = CardLocation.None;
        card.Zone = CardZone.None;
        card.MainDeckIndex = default;

        UpdateCountDisplay();

        return card;
    }
}
