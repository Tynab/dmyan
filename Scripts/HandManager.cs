using DMYAN.Scripts.Common;
using Godot;
using System.Collections.Generic;
using System.Threading.Tasks;
using static DMYAN.Scripts.Common.Constant;

namespace DMYAN.Scripts;

internal partial class HandManager : Node2D
{
    [Export]
    private DuelSide DuelSide { get; set; } = DuelSide.None;

    private readonly List<Card> _cardsInHand = [];

    internal async Task AddCardAsync(Card card)
    {
        _cardsInHand.Add(card);

        card.Reparent(this);

        card.Scale = CARD_IN_HAND_SCALE;
        card.Status = CardStatus.InHand;

        ArrangeCards();

        if (DuelSide is DuelSide.Player)
        {
            await card.AnimationDrawFlipAsync();
        }
    }

    internal void RemoveCard(Card card)
    {
        if (_cardsInHand.Remove(card))
        {
            ArrangeCards();
        }
    }

    private void ArrangeCards()
    {
        var space = _cardsInHand.Count * CARD_HAND_W + (_cardsInHand.Count - 1) * CARD_HAND_GAP_W <= HAND_AREA_MAX_W ? CARD_HAND_W + CARD_HAND_GAP_W : (HAND_AREA_MAX_W - CARD_HAND_W) / (_cardsInHand.Count - 1);

        for (var i = 0; i < _cardsInHand.Count; i++)
        {
            var newPosition = new Vector2((i - (_cardsInHand.Count - 1) / 2f) * space, HAND_POSITION_Y);
            var card = _cardsInHand[i];

            card.BasePosition = newPosition;
            card.ZIndex = i;

            card.AnimationDraw(newPosition);
        }
    }
}
