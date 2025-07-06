using Godot;
using YANLib;
using static DMYAN.Scripts.Common.Constant;

namespace DMYAN.Scripts.HandZoneStack;

internal partial class HandZone : Node2D
{
    private void ArrangeCardsAndResetIndex()
    {
        var cardsInHand = _gameManager.GetCardsInHand(DuelSide);

        if (cardsInHand.IsNullEmpty())
        {
            return;
        }

        var space = cardsInHand.Count * CARD_HAND_W + (cardsInHand.Count - 1) * CARD_HAND_GAP_W <= HAND_AREA_MAX_W ? CARD_HAND_W + CARD_HAND_GAP_W : (HAND_AREA_MAX_W - CARD_HAND_W) / (cardsInHand.Count - 1);

        for (var i = 0; i < cardsInHand.Count; i++)
        {
            var position = new Vector2((i - (cardsInHand.Count - 1) / 2f) * space, CARD_HAND_Y);
            var card = cardsInHand[i];

            card.BasePosition = position;
            card.ZIndex = i;
            card.HandIndex = i;

            card.AnimationDraw(position);
        }
    }
}
