using DMYAN.Scripts.CardStack;
using DMYAN.Scripts.Common.Enum;
using DMYAN.Scripts.Controls;
using Godot;
using Godot.Collections;
using System.Linq;
using System.Threading.Tasks;
using YANLib;
using static DMYAN.Scripts.Common.CardDatabase;
using static DMYAN.Scripts.Common.Constant;
using static Godot.FileAccess;
using static Godot.FileAccess.ModeFlags;
using static System.Threading.Tasks.Task;

namespace DMYAN.Scripts.GameManagerStack;

internal partial class GameManager : DMYANNode2D
{
    #region Prepared

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

    private async Task InitDraw()
    {
        for (var i = 0; i < INITIAL_HAND_SIZE; i++)
        {
            await DrawStep(PlayerMainDeck, PlayerHand);
            await DrawStep(OpponentMainDeck, OpponentHand);
        }
    }

    private async Task ChangeTurn()
    {
        CurrentTurnSide = CurrentTurnSide is DuelSide.Player ? DuelSide.Opponent : DuelSide.Player;
        IsFirstTurn = false;

        await ChangeVisibilityButtons();
    }

    #endregion

    #region Controls

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
                    await _phaseButtons[i + 1].ZoomOut();
                }
                else
                {
                    await _phaseButtons[i - 1].ZoomIn();
                }
            }
        }
        else
        {
            for (var i = 0; i < _phaseButtons.Count; i++)
            {
                if (i % 2 is 0)
                {
                    await _phaseButtons[i].ZoomOut();
                }
                else
                {
                    await _phaseButtons[i].ZoomIn();
                }
            }
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

    #endregion

    #region Validations

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

    #endregion

    #region Calculations

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

    private async Task AtkVsAtk(Card card)
    {
        if (CardAttacking.ATK > card.ATK)
        {
            var lpUpdate = card.ATK.Value - CardAttacking.ATK.Value;

            card.GetSlot().AnimationShowDamage(lpUpdate);

            await DestroyStep(card);
            await GetProfile(card.DuelSide).UpdateLifePoint(lpUpdate);
        }
        else if (CardAttacking.ATK < card.ATK)
        {
            var lpUpdate = CardAttacking.ATK.Value - card.ATK.Value;

            CardAttacking.GetSlot().AnimationShowDamage(lpUpdate);

            await DestroyStep(CardAttacking);
            await GetProfile(CardAttacking.DuelSide).UpdateLifePoint(lpUpdate);
        }
        else
        {
            await WhenAll(DestroyStep(card), DestroyStep(CardAttacking));
        }
    }

    private async Task AtkVsDef(Card card)
    {
        if (CardAttacking.ATK > card.DEF)
        {
            await DestroyStep(card, true);
        }
        else if (CardAttacking.ATK < card.DEF)
        {
            var lpUpdate = CardAttacking.ATK.Value - card.DEF.Value;

            CardAttacking.GetSlot().AnimationShowDamage(lpUpdate);

            await CardAttacking.AnimationAtkAttacked();
            await GetProfile(CardAttacking.DuelSide).UpdateLifePoint(lpUpdate);
        }
    }

    #endregion
}
