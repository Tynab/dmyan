using DMYAN.Scripts.CardStack;
using DMYAN.Scripts.Common;
using DMYAN.Scripts.Common.Enum;
using DMYAN.Scripts.GameManagerStack;
using Godot;
using System.Collections.Generic;
using System.Linq;
using static DMYAN.Scripts.Common.Constant;

namespace DMYAN.Scripts;

internal partial class MainDeck : CardSlot
{
    private GameManager _gameManager;
    private RichTextLabel _count;

    public override void _Ready()
    {
        var main = GetTree().Root.GetNode(MAIN_NODE);

        _gameManager = main.GetNode<GameManager>(nameof(GameManager));

        if (DuelSide is DuelSide.Player)
        {
            _count = GetNode<RichTextLabel>(SLOT_COUNT_NODE);
        }
    }

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
        card.MainDeckEnter(index);

        UpdateCountDisplay();
    }

    internal Card RemoveCard()
    {
        var cardsInMainDeck = _gameManager.GetCardsInMainDeck(DuelSide);

        if (cardsInMainDeck.Count is 0)
        {
            return default;
        }

        var card = cardsInMainDeck.OrderByDescending(x => x.MainDeckIndex).FirstOrDefault();

        card.Location = CardLocation.None;
        card.Zone = CardZone.None;
        card.MainDeckIndex = null;

        UpdateCountDisplay();

        return card;
    }

    private void ShuffleAndArrange()
    {
        var mainDeckIndices = _gameManager.GetMainDeckIndices(DuelSide);

        if (mainDeckIndices.Count is 0)
        {
            return;
        }

        mainDeckIndices.Shuffle();

        ArrangeCardsAndResetIndex(mainDeckIndices);
    }

    private void ArrangeCardsAndResetIndex(List<int> mainDeckIndices)
    {
        for (var i = 0; i < mainDeckIndices.Count; i++)
        {
            var card = _gameManager.Cards[i];

            card.ZIndex = mainDeckIndices[i];
            card.MainDeckIndex = mainDeckIndices[i];
        }
    }

    private void UpdateCountDisplay()
    {
        if (_count is not null)
        {
            var cardsInMainDeck = _gameManager.GetCardsInMainDeck(DuelSide);

            if (cardsInMainDeck.Count > 0)
            {
                _count.Text = cardsInMainDeck.Count.ToString();
                _count.ZIndex = 1000;
                _count.Show();
            }
            else
            {
                _count.Hide();
            }
        }
    }
}
