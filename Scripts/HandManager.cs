using Godot;
using System.Collections.Generic;
using System.Threading.Tasks;
using static DMYAN.Scripts.Constant;
using static Godot.Tween.EaseType;
using static Godot.Tween.TransitionType;

namespace DMYAN.Scripts;

public partial class HandManager : Node2D
{
    [Export]
    public DuelSide DuelSide { get; set; } = DuelSide.None;

    private readonly List<Card> _cardsInHand = [];

    public async Task AddCardToHand(Card card)
    {
        card.Reparent(this);
        card.Scale = new Vector2(CARD_HAND_SCALE, CARD_HAND_SCALE);
        card.Status = CardStatus.InHand;
        _cardsInHand.Add(card);
        ArrangeCardsInHand();

        if (DuelSide is DuelSide.Player)
        {
            await card.AnimationDrawFlipAsync();
        }
    }

    public void RemoveCardFromHand(Card card)
    {
        if (_cardsInHand.Remove(card))
        {
            ArrangeCardsInHand();
        }
    }

    private void ArrangeCardsInHand()
    {
        var space = _cardsInHand.Count * CARD_HAND_W + (_cardsInHand.Count - 1) * CARD_HAND_GAP_W <= HAND_AREA_MAX_W ? CARD_HAND_W + CARD_HAND_GAP_W : (HAND_AREA_MAX_W - CARD_HAND_W) / (_cardsInHand.Count - 1);

        for (var i = 0; i < _cardsInHand.Count; i++)
        {
            var card = _cardsInHand[i];
            var handPosition = new Vector2((i - (_cardsInHand.Count - 1) / 2f) * space, HAND_POSITION_Y);

            card.BasePosition = handPosition;
            card.AnimationDraw(handPosition);
            card.ZIndex = i;
        }
    }
}
