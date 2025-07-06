using DMYAN.Scripts.CardStack;
using DMYAN.Scripts.Common.Enum;
using DMYAN.Scripts.Controls;
using DMYAN.Scripts.HandZoneStack;
using DMYAN.Scripts.MainDeckStack;
using DMYAN.Scripts.MainZoneStack;
using Godot;
using Godot.Collections;
using System.Linq;
using System.Threading.Tasks;
using YANLib;
using static DMYAN.Scripts.Common.CardDatabase;
using static DMYAN.Scripts.Common.Constant;
using static Godot.FileAccess;
using static Godot.FileAccess.ModeFlags;

namespace DMYAN.Scripts.GameManagerStack;

internal partial class GameManager : Node2D
{
    private void LoadData()
    {
        Cards.Clear();

        LoadCardsData();

        InitCards(DuelSide.Player, DECKS_DATA_PATH);
        InitCards(DuelSide.Opponent, DECKS_DATA_PATH);

        PlayerMainDeck.Init([.. Cards.Where(static x => x.DuelSide is DuelSide.Player)]);
        OpponentMainDeck.Init([.. Cards.Where(static x => x.DuelSide is DuelSide.Opponent)]);
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

            if (line.IsNullWhiteSpace())
            {
                continue;
            }

            var parts = line.Split(',');
            var cardData = GetCardData(parts[1].Trim('"'));
            var card = CardScene.Instantiate<Card>();

            AddChild(card);

            card.Init(cardData, side);

            if (side is DuelSide.Opponent)
            {
                card.RotationDegrees = 180;
            }

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
            _ => default
        },
        DuelSide.Opponent => phase switch
        {
            DuelPhase.Draw => _opponentDPButton,
            DuelPhase.Standby => _opponentSPButton,
            DuelPhase.Main1 => _opponentM1Button,
            DuelPhase.Battle => _opponentBPButton,
            DuelPhase.Main2 => _opponentM2Button,
            DuelPhase.End => _opponentEPButton,
            _ => default
        },
        _ => default
    };

    private void SetupVisibilityButtons()
    {
        if (CurrentTurnSide is DuelSide.Player)
        {
            for (var i = 0; i < _phaseButtons.Count; i++)
            {
                if (i % 2 is 0)
                {
                    _phaseButtons[i].ChangeStatus(true);
                    _phaseButtons[i].Show();
                }
                else
                {
                    _phaseButtons[i].Hide();
                    _phaseButtons[i].ChangeStatus(false);
                }
            }
        }
        else
        {
            for (var i = 0; i < _phaseButtons.Count; i++)
            {
                if (i % 2 is 0)
                {
                    _phaseButtons[i + 1].ChangeStatus(true);
                    _phaseButtons[i + 1].Show();
                }
                else
                {
                    _phaseButtons[i - 1].Hide();
                    _phaseButtons[i - 1].ChangeStatus(false);
                }
            }
        }
    }

    private async Task ChangeVisibilityButtons()
    {
        if (CurrentTurnSide is DuelSide.Player)
        {
            for (var i = 0; i < _phaseButtons.Count; i++)
            {
                if (i % 2 is 0)
                {
                    await _phaseButtons[i + 1].Out();
                }
                else
                {
                    await _phaseButtons[i - 1].In();
                }
            }
        }
        else
        {
            for (var i = 0; i < _phaseButtons.Count; i++)
            {
                if (i % 2 is 0)
                {
                    await _phaseButtons[i].Out();
                }
                else
                {
                    await _phaseButtons[i].In();
                }
            }
        }
    }

    private async Task StartInitialDrawAsync()
    {
        for (var i = 0; i < INITIAL_HAND_SIZE; i++)
        {
            await DrawStepAsync(PlayerMainDeck, PlayerHand);
            await DrawStepAsync(OpponentMainDeck, OpponentHand);
        }
    }

    private async Task DrawStepAsync(MainDeck deck, HandZone hand)
    {
        CurrentStep = DuelStep.Drawing;

        var card = deck.RemoveCard();

        if (card.IsNotNull())
        {
            await hand.AddCardAsync(card);
        }

        CurrentStep = DuelStep.Drawn;
    }

    private async Task SummonStep(Card card, HandZone hand, MainZone zone)
    {
        CurrentStep = DuelStep.Summoning;

        hand.RemoveCard(card);

        await zone.SummonCard(card);

        HasSummoned = true;
        CurrentStep = DuelStep.Summoned;

        CannotSummonOrSet();
    }

    private void SetSummonStep(Card card, HandZone hand, MainZone zone)
    {
        CurrentStep = DuelStep.SetSummoning;

        hand.RemoveCard(card);
        zone.SummonSetCard(card);

        HasSummoned = true;
        CurrentStep = DuelStep.SetSummoned;

        CannotSummonOrSet();
    }

    private async Task AttackStep(Card card)
    {
        await CardAttacking.Sword.AnimationAttack(card.GlobalPosition);

        if (card.CardFace is CardFace.FaceUp)
        {
            if (card.CardPosition is CardPosition.Attack)
            {
                await AtkVsAtk(card);
            }
            else if (card.CardPosition is CardPosition.Defense)
            {
                await AtkVsDef(card);
            }
        }
        else if (card.CardFace is CardFace.FaceDown)
        {
            await card.FlipUp();

            await AtkVsDef(card);
        }

        CardAttacking = default;
        CurrentStep = DuelStep.Attacked;
    }

    private async Task DestroyStep(Card card)
    {
        CurrentStep = DuelStep.Destroying;

        await card.Destroy();
        await GetGraveyard(card.DuelSide).AddCard(card, Cards.Count(x => x.DuelSide == card.DuelSide && x.Location is CardLocation.InBoard && x.Zone is CardZone.Graveyard) + 1);
    }

    private async Task AtkVsAtk(Card card)
    {
        if (CardAttacking.ATK > card.ATK)
        {
            GetProfile(card.DuelSide).UpdateLifePoint(card.ATK.Value - CardAttacking.ATK.Value);

            await DestroyStep(card);
        }
        else if (CardAttacking.ATK < card.ATK)
        {
            GetProfile(CardAttacking.DuelSide).UpdateLifePoint(CardAttacking.ATK.Value - card.ATK.Value);

            await DestroyStep(CardAttacking);
        }
        else
        {
            await DestroyStep(card);
            await DestroyStep(CardAttacking);
        }
    }

    private async Task AtkVsDef(Card card)
    {
        if (CardAttacking.ATK > card.DEF)
        {
            await DestroyStep(card);
        }
        else if (CardAttacking.ATK < card.DEF)
        {
            GetProfile(CardAttacking.DuelSide).UpdateLifePoint(CardAttacking.ATK.Value - card.DEF.Value);
        }
    }

    private void CanSummonOrSet()
    {
        if (!HasSummoned)
        {
            Cards.Where(x => x.DuelSide == CurrentTurnSide && x.Location is CardLocation.InHand).ToList().ForEach(x => x.CanSummonOrSetCheck());
        }
    }

    private void CannotSummonOrSet()
    {
        if (HasSummoned)
        {
            Cards.Where(x => x.DuelSide == CurrentTurnSide && x.Location is CardLocation.InHand).ToList().ForEach(x => x.CannotSummonOrSetCheck());
        }
    }

    private Card GetCardAtCursor()
    {
        var results = GetWorld2D().DirectSpaceState.IntersectPoint(new PhysicsPointQueryParameters2D
        {
            Position = GetGlobalMousePosition(),
            CollideWithAreas = true,
            CollisionMask = CARD_COLLISION_MASK
        });

        return results.IsNullEmpty() ? default : GetTopmostCard(results);
    }

    private static Card GetTopmostCard(Array<Dictionary> cards) => cards.Select(static c => c[COLLIDER_PROPERTY].As<Area2D>().GetParent<Card>()).OrderByDescending(static c => c.ZIndex).FirstOrDefault();

    private async Task ChangeTurnAsync()
    {
        CurrentTurnSide = CurrentTurnSide is DuelSide.Player ? DuelSide.Opponent : DuelSide.Player;
        IsFirstTurn = false;

        await ChangeVisibilityButtons();
    }

    private int? GetMinAtkInMainBySide(DuelSide side)
    {
        var cards = GetCardsInMainZone(side);

        return cards.IsNullEmpty() ? default : cards.Min(static x => x.ATK);
    }

    private int? GetMaxAtkInMainBySide(DuelSide side)
    {
        var cards = GetCardsInMainZone(side);

        return cards.IsNullEmpty() ? default : cards.Max(static x => x.ATK);
    }
}
