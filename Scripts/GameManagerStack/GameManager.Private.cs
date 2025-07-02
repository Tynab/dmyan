using DMYAN.Scripts.CardStack;
using DMYAN.Scripts.Common;
using DMYAN.Scripts.Common.Enum;
using DMYAN.Scripts.Controls;
using Godot;
using Godot.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static DMYAN.Scripts.Common.CardDatabase;
using static DMYAN.Scripts.Common.Constant;
using static Godot.FileAccess;
using static Godot.FileAccess.ModeFlags;
using static Godot.ResourceLoader;
using static Godot.Vector2;
using static System.Threading.Tasks.Task;

namespace DMYAN.Scripts.GameManagerStack;

internal partial class GameManager : Node2D
{
    private void LoadData()
    {
        Cards.Clear();

        LoadCardsData();

        InitCards(DuelSide.Player, DECKS_DATA_PATH);
        InitCards(DuelSide.Opponent, DECKS_DATA_PATH);

        PlayerMainDeck.Init([.. Cards.Where(x => x.DuelSide is DuelSide.Player)]);
        OpponentMainDeck.Init([.. Cards.Where(x => x.DuelSide is DuelSide.Opponent)]);
    }

    private void InitCards(DuelSide side, string path)
    {
        using var file = Open(path, Read);

        if (!file.EofReached())
        {
            _ = file.GetLine();
        }

        while (!file.EofReached())
        {
            var line = file.GetLine();

            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            var parts = line.Split(',');
            var cardData = GetCardData(parts[1].Trim('"'));
            var card = CardScene.Instantiate<Card>();

            card.Init(cardData, side);

            Cards.Add(card);
        }
    }

    private PhaseButton GetPhaseButton(DuelSide side, DuelPhase phase) => side switch
    {
        DuelSide.Player => phase switch
        {
            DuelPhase.Draw => PlayerDP,
            DuelPhase.Standby => PlayerSP,
            DuelPhase.Main1 => PlayerM1,
            DuelPhase.Battle => PlayerBP,
            DuelPhase.Main2 => PlayerM2,
            DuelPhase.End => PlayerEP,
            _ => null
        },
        DuelSide.Opponent => phase switch
        {
            DuelPhase.Draw => OpponentDP,
            DuelPhase.Standby => OpponentSP,
            DuelPhase.Main1 => OpponentM1,
            DuelPhase.Battle => OpponentBP,
            DuelPhase.Main2 => OpponentM2,
            DuelPhase.End => OpponentEP,
            _ => null
        },
        _ => null
    };

    private async Task ChangeVisibilityButtons(DuelSide side)
    {
        if (side is DuelSide.Player)
        {
            PlayerDP.ChangeStatus(true);
            PlayerDP.Show();
            OpponentDP.Hide();
            OpponentDP.ChangeStatus(false);

            await Delay(STARTUP_DELAY);

            PlayerSP.ChangeStatus(true);
            PlayerSP.Show();
            OpponentSP.Hide();
            OpponentSP.ChangeStatus(false);

            await Delay(STARTUP_DELAY);

            PlayerM1.ChangeStatus(true);
            PlayerM1.Show();
            OpponentM1.Hide();
            OpponentM1.ChangeStatus(false);

            await Delay(STARTUP_DELAY);

            PlayerBP.ChangeStatus(true);
            PlayerBP.Show();
            OpponentBP.Hide();
            OpponentBP.ChangeStatus(false);

            await Delay(STARTUP_DELAY);

            PlayerM2.ChangeStatus(true);
            PlayerM2.Show();
            OpponentM2.Hide();
            OpponentM2.ChangeStatus(false);

            await Delay(STARTUP_DELAY);

            PlayerEP.ChangeStatus(true);
            PlayerEP.Show();
            OpponentEP.Hide();
            OpponentEP.ChangeStatus(false);

            await Delay(STARTUP_DELAY);
        }
        else
        {
            OpponentDP.ChangeStatus(true);
            OpponentDP.Show();
            PlayerDP.Hide();
            PlayerDP.ChangeStatus(false);

            await Delay(STARTUP_DELAY);

            OpponentSP.ChangeStatus(true);
            OpponentSP.Show();
            PlayerSP.Hide();
            PlayerSP.ChangeStatus(false);

            await Delay(STARTUP_DELAY);

            OpponentM1.ChangeStatus(true);
            OpponentM1.Show();
            PlayerM1.Hide();
            PlayerM1.ChangeStatus(false);

            await Delay(STARTUP_DELAY);

            OpponentBP.ChangeStatus(true);
            OpponentBP.Show();
            PlayerBP.Hide();
            PlayerBP.ChangeStatus(false);

            await Delay(STARTUP_DELAY);

            OpponentM2.ChangeStatus(true);
            OpponentM2.Show();
            PlayerM2.Hide();
            PlayerM2.ChangeStatus(false);

            await Delay(STARTUP_DELAY);

            OpponentEP.ChangeStatus(true);
            OpponentEP.Show();
            PlayerEP.Hide();
            PlayerEP.ChangeStatus(false);
        }
    }

    private async Task StartInitialDrawAsync()
    {
        for (var i = 0; i < INITIAL_HAND_SIZE; i++)
        {
            await DrawAndPlaceCardAsync(PlayerMainDeck, PlayerHand);
            await DrawAndPlaceCardAsync(OpponentMainDeck, OpponentHand);
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
        CurrentTurnSide = CurrentTurnSide is DuelSide.Player ? DuelSide.Opponent : DuelSide.Player;
        IsFirstTurn = false;

        await ChangeVisibilityButtons(CurrentTurnSide);
    }
}
