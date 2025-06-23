using Godot;
using System.Collections.Generic;
using System.Threading.Tasks;
using static DMYAN.Scripts.Constant;

namespace DMYAN.Scripts;

public partial class HandManager : Node2D
{
    [Export]
    public DuelSide DuelSide { get; set; } = DuelSide.None;

    private readonly List<Card> _cardsInHand = [];

    public async Task AddCard(Card card)
    {
        _cardsInHand.Add(card);
        card.Reparent(this);
        card.Scale = new Vector2(CARD_HAND_SCALE, CARD_HAND_SCALE);
        card.Status = CardStatus.InHand;
        ArrangeCards();

        if (DuelSide is DuelSide.Player)
        {
            await card.AnimationDrawFlipAsync();
        }
    }

    public void RemoveCard(Card card)
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
            var card = _cardsInHand[i];
            var newPosition = new Vector2((i - (_cardsInHand.Count - 1) / 2f) * space, HAND_POSITION_Y);

            card.BasePosition = newPosition;
            card.AnimationDraw(newPosition);
            card.ZIndex = i;
        }
    }
}
