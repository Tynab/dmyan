using DMYAN.Scripts.CardStack;
using DMYAN.Scripts.Common.Enum;
using DMYAN.Scripts.GameManagerStack;
using Godot;
using System.Collections.Generic;
using System.Threading.Tasks;
using static DMYAN.Scripts.Common.Constant;

namespace DMYAN.Scripts;

internal partial class HandManager : Node2D
{
    [Export]
    private DuelSide DuelSide { get; set; } = DuelSide.None;

    private GameManager _gameManager;

    public override void _Ready() => _gameManager = GetParent().GetParent().GetParent().GetNode<GameManager>(nameof(GameManager));

    internal async Task AddCardAsync(Card card)
    {
        card.Reparent(this);

        card.Scale = SCALE_MAX;
        card.Location = CardLocation.InHand;
        card.HandIndex = GetChildCount();

        //ArrangeCards();

        //if (DuelSide is DuelSide.Player)
        //{
        //    await card.AnimationDrawFlipAsync();
        //}
    }

    internal void RemoveCard(Card card)
    {
        //if (_cardsInHand.Remove(card))
        //{
        //    ArrangeCards();
        //}
    }

    private void ArrangeCards()
    {
        var cardsInHand = _gameManager.GetCardsInHand(DuelSide);
        var space = cardsInHand.Count * CARD_HAND_W + (cardsInHand.Count - 1) * CARD_HAND_GAP_W <= HAND_AREA_MAX_W ? CARD_HAND_W + CARD_HAND_GAP_W : (HAND_AREA_MAX_W - CARD_HAND_W) / (cardsInHand.Count - 1);

        for (var i = 0; i < cardsInHand.Count; i++)
        {
            var position = new Vector2((i - (cardsInHand.Count - 1) / 2f) * space, HAND_POSITION_Y);
            var card = cardsInHand[i];

            card.BasePosition = position;
            card.ZIndex = i;

            card.AnimationDraw(position);
        }
    }
}
