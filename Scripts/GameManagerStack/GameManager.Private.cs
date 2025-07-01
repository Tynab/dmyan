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

        LoadCards();

        LoadMainDeck(DuelSide.Player, DECKS_DATA_PATH);
        LoadMainDeck(DuelSide.Opponent, DECKS_DATA_PATH);

        //Cards.Where(x => x.DuelSide == DuelSide.Player).ToList().ForEach(PlayerDeck.AddCard);
        Cards.Where(x => x.DuelSide == DuelSide.Opponent).ToList().ForEach(OpponentMainDeck.AddCard);
    }

    private void LoadMainDeck(DuelSide side, string path)
    {
        using var file = Open(path, Read);

        if (!file.EofReached())
        {
            _ = file.GetLine();
        }

        var i = 0;

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

            card.BaseSide = side;
            card.DuelSide = side;
            card.Location = CardLocation.InDeck;
            card.Zone = CardZone.MainDeck;
            card.CardFace = CardFace.FaceDown;
            card.CardPosition = CardPosition.None;
            card.Type = cardData.Type;
            card.Property = cardData.Property;
            card.Attribute = cardData.Attribute;
            card.Race = cardData.Race;
            card.SummonType = cardData.SummonType;
            card.BanlistStatus = cardData.BanlistStatus;
            card.EffectType = cardData.EffectType;
            card.MainDeckIndex = i;
            card.ExtraDeckIndex = null;
            card.HandIndex = null;
            card.MainIndex = null;
            card.STPIndex = null;
            card.GraveyardIndex = null;
            card.BanishedIndex = null;
            card.Code = cardData.Code;
            card.CardName = cardData.Name;
            card.Description = cardData.Description;
            card.Level = cardData.Level;
            card.BaseATK = cardData.ATK;
            card.BaseDEF = cardData.DEF;
            card.ATK = cardData.ATK;
            card.DEF = cardData.DEF;
            card.BasePosition = Zero;
            card.CanActivate = false;
            card.CanSummon = false;
            card.CanSet = false;
            card.CanAttack = false;
            card.CanDirectAttack = false;
            card.ActionType = CardActionType.None;

            var cardFront = card.GetNode<Sprite2D>(CARD_FRONT_NODE);

            cardFront.Texture = Load<Texture2D>(cardData.Code.GetCardAssetPathByCode());
            cardFront.Hide();

            var cardBack = card.GetNode<Sprite2D>(CARD_BACK_NODE);

            cardBack.Show();

            Cards.Add(card);

            i++;
        }
    }

    private PhaseButton GetPhaseButton(DuelSide side, DuelPhase phase) => side switch
    {
        DuelSide.Player => phase switch
        {
            DuelPhase.Draw => _playerDpButton,
            DuelPhase.Standby => _playerSpButton,
            DuelPhase.Main1 => _playerM1Button,
            DuelPhase.Battle => _playerBpButton,
            DuelPhase.Main2 => _playerM2Button,
            DuelPhase.End => _playerEpButton,
            _ => null
        },
        DuelSide.Opponent => phase switch
        {
            DuelPhase.Draw => _opponentDpButton,
            DuelPhase.Standby => _opponentSpButton,
            DuelPhase.Main1 => _opponentM1Button,
            DuelPhase.Battle => _opponentBpButton,
            DuelPhase.Main2 => _opponentM2Button,
            DuelPhase.End => _opponentEpButton,
            _ => null
        },
        _ => null
    };

    private async Task ChangeVisibilityButtons(DuelSide side)
    {
        if (side is DuelSide.Player)
        {
            _playerDpButton.ChangeStatus(true);
            _playerDpButton.Show();
            _opponentDpButton.Hide();
            _opponentDpButton.ChangeStatus(false);

            await Delay(STARTUP_DELAY);

            _playerSpButton.ChangeStatus(true);
            _playerSpButton.Show();
            _opponentSpButton.Hide();
            _opponentSpButton.ChangeStatus(false);

            await Delay(STARTUP_DELAY);

            _playerM1Button.ChangeStatus(true);
            _playerM1Button.Show();
            _opponentM1Button.Hide();
            _opponentM1Button.ChangeStatus(false);

            await Delay(STARTUP_DELAY);

            _playerBpButton.ChangeStatus(true);
            _playerBpButton.Show();
            _opponentBpButton.Hide();
            _opponentBpButton.ChangeStatus(false);

            await Delay(STARTUP_DELAY);

            _playerM2Button.ChangeStatus(true);
            _playerM2Button.Show();
            _opponentM2Button.Hide();
            _opponentM2Button.ChangeStatus(false);

            await Delay(STARTUP_DELAY);

            _playerEpButton.ChangeStatus(true);
            _opponentEpButton.Hide();
            _playerEpButton.Show();
            _opponentEpButton.ChangeStatus(false);

            await Delay(STARTUP_DELAY);
        }
        else
        {
            _opponentDpButton.ChangeStatus(true);
            _opponentDpButton.Show();
            _playerDpButton.Hide();
            _playerDpButton.ChangeStatus(false);

            await Delay(STARTUP_DELAY);

            _opponentSpButton.ChangeStatus(true);
            _opponentSpButton.Show();
            _playerSpButton.Hide();
            _playerSpButton.ChangeStatus(false);

            await Delay(STARTUP_DELAY);

            _opponentM1Button.ChangeStatus(true);
            _opponentM1Button.Show();
            _playerM1Button.Hide();
            _playerM1Button.ChangeStatus(false);

            await Delay(STARTUP_DELAY);

            _opponentBpButton.ChangeStatus(true);
            _opponentBpButton.Show();
            _playerBpButton.Hide();
            _playerBpButton.ChangeStatus(false);

            await Delay(STARTUP_DELAY);

            _opponentM2Button.ChangeStatus(true);
            _opponentM2Button.Show();
            _playerM2Button.Hide();
            _playerM2Button.ChangeStatus(false);

            await Delay(STARTUP_DELAY);

            _opponentEpButton.ChangeStatus(true);
            _opponentEpButton.Show();
            _playerEpButton.Hide();
            _playerEpButton.ChangeStatus(false);
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
