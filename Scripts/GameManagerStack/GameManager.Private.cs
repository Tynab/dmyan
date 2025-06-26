using DMYAN.Scripts.Common;
using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DMYAN.Scripts.Common.CardDatabase;
using static DMYAN.Scripts.Common.Constant;
using static Godot.MouseButton;
using static System.Threading.Tasks.Task;

namespace DMYAN.Scripts.GameManagerStack;

internal partial class GameManager : Node2D
{
    private async Task StartInitialDrawAsync()
    {
        for (var i = 0; i < INITIAL_HAND_SIZE; i++)
        {
            await DrawAndPlaceCardAsync(PlayerDeck, PlayerHand);
            await DrawAndPlaceCardAsync(OpponentDeck, OpponentHand);
        }
    }

    private static async Task DrawAndPlaceCardAsync(MainDeck deck, HandManager hand)
    {
        var card = deck.RemoveCard();

        if (card is not null)
        {
            await hand.AddCardAsync(card);
        }
    }

    private void SummonAndPlaceCard(Card card, HandManager hand, MainZone zone)
    {
        HasSummoned = true;

        hand.RemoveCard(card);
        zone.SummonCard(card);
    }

    private void SummonSetAndPlaceCard(Card card, HandManager hand, MainZone zone)
    {
        HasSummoned = true;

        hand.RemoveCard(card);
        zone.SummonSetCard(card);
    }

    private Card GetCardAtCursor()
    {
        var results = GetWorld2D().DirectSpaceState.IntersectPoint(new PhysicsPointQueryParameters2D
        {
            Position = GetGlobalMousePosition(),
            CollideWithAreas = true,
            CollisionMask = CARD_COLLISION_MASK
        });

        return results.Count > 0 ? GetTopmostCard(results) : default;
    }

    private static Card GetTopmostCard(Array<Dictionary> cards) => cards.Select(static c => c[COLLIDER_PROPERTY].As<Area2D>().GetParent<Card>()).OrderByDescending(static c => c.ZIndex).FirstOrDefault();

    private async Task ChangeTurnAsync()
    {
        _dpButton.ChangeStatus(true);
        await Delay(STARTUP_DELAY);
        _spButton.ChangeStatus(true);
        await Delay(STARTUP_DELAY);
        _m1Button.ChangeStatus(true);
        await Delay(STARTUP_DELAY);
        _bpButton.ChangeStatus(true);
        await Delay(STARTUP_DELAY);
        _m2Button.ChangeStatus(true);
        await Delay(STARTUP_DELAY);
        _epButton.ChangeStatus(true);
        await Delay(STARTUP_DELAY);

        CurrentTurnSide = CurrentTurnSide is DuelSide.Player ? DuelSide.Opponent : DuelSide.Player;
        IsFirstTurn = false;

        await HighlightEye();
    }

    internal async Task HighlightEye()
    {
        if (CurrentTurnSide is DuelSide.Player)
        {
            await _opponentEye.Hide(OPACITY_MIN, SCALE_MIN);
            await _playerEye.Show(OPACITY_MAX, SCALE_MAX);
        }
        else
        {
            await _playerEye.Hide(OPACITY_MIN, SCALE_MIN);
            await _opponentEye.Show(OPACITY_MAX, SCALE_MAX);
        }
    }
}
