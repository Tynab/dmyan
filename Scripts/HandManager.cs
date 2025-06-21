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

    public List<Card> CardsInHand { get; set; } = [];

    public async Task AddCardToHand(Card card)
    {
        card.Reparent(this);
        card.Status = CardStatus.InHand;
        card.Zone = CardZone.None;
        card.Scale = new Vector2(CARD_HAND_SCALE, CARD_HAND_SCALE);
        CardsInHand.Add(card);
        ArrangeCardsInHand();

        if (DuelSide is DuelSide.Player)
        {
            await card.PlayFlipAnimationAsync();
        }
    }

    private void ArrangeCardsInHand()
    {
        for (var i = 0; i < CardsInHand.Count; i++)
        {
            var card = CardsInHand[i];

            AnimateCardPositions(card, new Vector2(HAND_POSITION_X + (i - (CardsInHand.Count - 1) / 2f) * (CARD_HAND_W + CARD_HAND_GAP), HAND_POSITION_Y), 0.2);
            card.ZIndex = i;
        }
    }

    private void AnimateCardPositions(Card card, Vector2 position, double speed) => GetTree().CreateTween().SetTrans(Circ).SetEase(Out).TweenProperty(card, "position", position, speed);
}
