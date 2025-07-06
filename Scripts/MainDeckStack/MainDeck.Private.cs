using DMYAN.Scripts.Common;
using System.Collections.Generic;
using YANLib;

namespace DMYAN.Scripts.MainDeckStack;

internal partial class MainDeck : CardSlot
{
    private void ShuffleAndArrange()
    {
        var indices = _gameManager.GetMainDeckIndices(DuelSide);

        if (indices.IsNullEmpty())
        {
            return;
        }

        indices.Shuffle();

        ArrangeCardsAndResetIndex(indices);
    }

    private void ArrangeCardsAndResetIndex(List<int> indices)
    {
        var cards = _gameManager.GetCards(DuelSide);

        for (var i = 0; i < indices.Count; i++)
        {
            var card = cards[i];

            card.ZIndex = indices[i];
            card.MainDeckIndex = indices[i];
        }
    }

    private void UpdateCountDisplay()
    {
        if (_count.IsNotNull())
        {
            var cards = _gameManager.GetCardsInMainDeck(DuelSide);

            if (cards.IsNullEmpty())
            {
                _count.Hide();
            }
            else
            {
                _count.Text = cards.Count.ToString();
                _count.ZIndex = 1_000;
                _count.Show();
            }
        }
    }
}
