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

            AddChild(card);

            card.Init(cardData, side);
            card.Id = Cards.Count + 1;

            Cards.Add(card);
        }
    }

    private PhaseButton GetPhaseButton(DuelSide side, DuelPhase phase) => side switch
    {
        DuelSide.Player => phase switch
        {
            DuelPhase.Draw => _playerDPButton,
            DuelPhase.Standby => _playerSPButton,
            DuelPhase.Main1 => _playerM1Button,
            DuelPhase.Battle => _playerBPButton,
            DuelPhase.Main2 => _playerM2Button,
            DuelPhase.End => _playerEPButton,
            _ => null
        },
        DuelSide.Opponent => phase switch
        {
            DuelPhase.Draw => _opponentDPButton,
            DuelPhase.Standby => _opponentSPButton,
            DuelPhase.Main1 => _opponentM1Button,
            DuelPhase.Battle => _opponentBPButton,
            DuelPhase.Main2 => _opponentM2Button,
            DuelPhase.End => _opponentEPButton,
            _ => null
        },
        _ => null
    };

    private async Task ChangeVisibilityButtons(DuelSide side)
    {
        if (side is DuelSide.Player)
        {
            _playerDPButton.ChangeStatus(true);
            _playerDPButton.Show();
            _opponentDPButton.Hide();
            _opponentDPButton.ChangeStatus(false);

            await Delay(STARTUP_DELAY);

            _playerSPButton.ChangeStatus(true);
            _playerSPButton.Show();
            _opponentSPButton.Hide();
            _opponentSPButton.ChangeStatus(false);

            await Delay(STARTUP_DELAY);

            _playerM1Button.ChangeStatus(true);
            _playerM1Button.Show();
            _opponentM1Button.Hide();
            _opponentM1Button.ChangeStatus(false);

            await Delay(STARTUP_DELAY);

            _playerBPButton.ChangeStatus(true);
            _playerBPButton.Show();
            _opponentBPButton.Hide();
            _opponentBPButton.ChangeStatus(false);

            await Delay(STARTUP_DELAY);

            _playerM2Button.ChangeStatus(true);
            _playerM2Button.Show();
            _opponentM2Button.Hide();
            _opponentM2Button.ChangeStatus(false);

            await Delay(STARTUP_DELAY);

            _playerEPButton.ChangeStatus(true);
            _playerEPButton.Show();
            _opponentEPButton.Hide();
            _opponentEPButton.ChangeStatus(false);

            await Delay(STARTUP_DELAY);
        }
        else
        {
            _opponentDPButton.ChangeStatus(true);
            _opponentDPButton.Show();
            _playerDPButton.Hide();
            _playerDPButton.ChangeStatus(false);

            await Delay(STARTUP_DELAY);

            _opponentSPButton.ChangeStatus(true);
            _opponentSPButton.Show();
            _playerSPButton.Hide();
            _playerSPButton.ChangeStatus(false);

            await Delay(STARTUP_DELAY);

            _opponentM1Button.ChangeStatus(true);
            _opponentM1Button.Show();
            _playerM1Button.Hide();
            _playerM1Button.ChangeStatus(false);

            await Delay(STARTUP_DELAY);

            _opponentBPButton.ChangeStatus(true);
            _opponentBPButton.Show();
            _playerBPButton.Hide();
            _playerBPButton.ChangeStatus(false);

            await Delay(STARTUP_DELAY);

            _opponentM2Button.ChangeStatus(true);
            _opponentM2Button.Show();
            _playerM2Button.Hide();
            _playerM2Button.ChangeStatus(false);

            await Delay(STARTUP_DELAY);

            _opponentEPButton.ChangeStatus(true);
            _opponentEPButton.Show();
            _playerEPButton.Hide();
            _playerEPButton.ChangeStatus(false);
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
