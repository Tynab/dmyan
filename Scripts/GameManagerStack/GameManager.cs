using DMYAN.Scripts.CardStack;
using DMYAN.Scripts.Common.Enum;
using Godot;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YANLib;
using static DMYAN.Scripts.Common.Constant;
using static System.Threading.Tasks.Task;

namespace DMYAN.Scripts.GameManagerStack;

internal partial class GameManager : Node2D
{
    internal static DuelSide GetOpposite(DuelSide side) => side switch
    {
        DuelSide.Player => DuelSide.Opponent,
        DuelSide.Opponent => DuelSide.Player,
        _ => DuelSide.None
    };

    internal List<Card> GetCardsInMainDeck(DuelSide side) => [.. Cards.Where(x => x.DuelSide == side && x.Location == CardLocation.InDeck && x.Zone == CardZone.MainDeck && x.MainDeckIndex.HasValue)];

    internal List<int> GetMainDeckIndices(DuelSide side) => [.. GetCardsInMainDeck(side).Select(static x => x.MainDeckIndex.Value)];

    internal List<Card> GetCardsInExtraDeck(DuelSide side) => [.. Cards.Where(x => x.DuelSide == side && x.Location == CardLocation.InDeck && x.Zone == CardZone.ExtraDeck && x.ExtraDeckIndex.HasValue)];

    internal List<int> GetExtraDeckIndices(DuelSide side) => [.. GetCardsInExtraDeck(side).Select(static x => x.ExtraDeckIndex.Value)];

    internal List<Card> GetCardsInHand(DuelSide side) => [.. Cards.Where(x => x.DuelSide == side && x.Location == CardLocation.InHand && x.HandIndex.HasValue)];

    internal List<int> GetHandIndices(DuelSide side) => [.. GetCardsInHand(side).Select(static x => x.HandIndex.Value)];

    internal List<Card> GetCardInHandCanSummon(DuelSide side) => [.. Cards.Where(x => x.DuelSide == side && x.Location == CardLocation.InHand && x.HandIndex.HasValue && x.CanSummon)];

    internal List<Card> GetCardsInMainZone(DuelSide side) => [.. Cards.Where(x => x.DuelSide == side && x.Location == CardLocation.InBoard && x.Zone == CardZone.Main)];

    internal Card GetHigherAtkCardInHand(DuelSide side)
    {
        Card result = default;
        var cards = GetCardInHandCanSummon(side);
        var maxAtk = GetMaxAtkInMainBySide(GetOpposite(side));

        if (maxAtk.HasValue)
        {
            result = cards.Where(x => x.ATK >= maxAtk).OrderBy(x => x.DEF).FirstOrDefault();

            if (result.IsNull())
            {
                var minAtk = GetMinAtkInMainBySide(GetOpposite(side));

                if (minAtk.HasValue)
                {
                    result = cards.Where(x => x.ATK > minAtk).OrderBy(x => x.DEF).FirstOrDefault();
                }
            }
        }

        return result;
    }

    internal Card GetHighestAtkCardInHand(DuelSide side) => Cards.Where(x => x.DuelSide == side && x.Location is CardLocation.InHand && x.CanSummon).OrderByDescending(x => x.ATK).FirstOrDefault();

    internal Card GetHighestDefCardInHand(DuelSide side) => Cards.Where(x => x.DuelSide == side && x.Location is CardLocation.InHand && x.CanSet).OrderByDescending(x => x.DEF).FirstOrDefault();

    internal bool HasCardInMainZone(DuelSide side) => Cards.Any(x => x.DuelSide == side && x.Location is CardLocation.InBoard && x.Zone is CardZone.Main);

    internal Vector2 GetAvatarPosition(DuelSide side) => side switch
    {
        DuelSide.Player => PlayerInfo.GetAvatarPosition(),
        DuelSide.Opponent => OpponentInfo.GetAvatarPosition(),
        _ => Vector2.Zero
    };

    internal async Task DrawPhaseAsync(int delay = PHASE_CHANGE_DELAY)
    {
        CurrentPhase = DuelPhase.Draw;
        HasSummoned = false;

        GetPhaseButton(CurrentTurnSide, CurrentPhase).ChangeStatus(false);

        await _popupPhase.ShowPhase(CurrentTurnSide, CurrentPhase);

        if (CurrentTurnSide is DuelSide.Player)
        {
            await DrawStepAsync(PlayerMainDeck, PlayerHand);
        }
        else
        {
            await DrawStepAsync(OpponentMainDeck, OpponentHand);
        }

        await Delay(delay);

        await StandbyPhaseAsync();
    }

    internal async Task StandbyPhaseAsync(int delay = PHASE_CHANGE_DELAY)
    {
        CurrentPhase = DuelPhase.Standby;

        GetPhaseButton(CurrentTurnSide, CurrentPhase).ChangeStatus(false);

        await _popupPhase.ShowPhase(CurrentTurnSide, CurrentPhase);

        await Delay(delay);

        await Main1PhaseAsync();
    }

    internal async Task Main1PhaseAsync(int delay = PHASE_CHANGE_DELAY)
    {
        CurrentPhase = DuelPhase.Main1;

        GetPhaseButton(CurrentTurnSide, CurrentPhase).ChangeStatus(false);

        await _popupPhase.ShowPhase(CurrentTurnSide, CurrentPhase);

        CardInHandCanSummonOrSet();

        if (CurrentTurnSide is DuelSide.Opponent)
        {
            await this.OpponentSummonOrSetCard();
        }

        await Delay(delay);
    }

    internal async Task BattlePhaseAsync(int delay = PHASE_CHANGE_DELAY)
    {
        CurrentPhase = DuelPhase.Battle;

        GetPhaseButton(CurrentTurnSide, CurrentPhase).ChangeStatus(false);

        await _popupPhase.ShowPhase(CurrentTurnSide, CurrentPhase);

        if (!IsFirstTurn)
        {
            Cards.ForEach(async x => await x.CanAttackCheck(CurrentTurnSide));
        }

        await Delay(delay);
    }

    internal async Task Main2PhaseAsync(int delay = PHASE_CHANGE_DELAY)
    {
        CurrentPhase = DuelPhase.Main2;

        Cards.Where(x => x.DuelSide == CurrentTurnSide && x.Location is CardLocation.InBoard && x.Zone is CardZone.Main && x.CardFace is CardFace.FaceUp && x.CardPosition is CardPosition.Attack)
            .ToList()
            .ForEach(async x => await x.Sword.Hide(OPACITY_MIN));

        GetPhaseButton(CurrentTurnSide, CurrentPhase).ChangeStatus(false);

        await _popupPhase.ShowPhase(CurrentTurnSide, CurrentPhase);

        await Delay(delay);
    }

    internal async Task EndPhaseAsync(int delay = STARTUP_DELAY)
    {
        if (CurrentPhase is DuelPhase.Main1)
        {
            await BattlePhaseAsync(STARTUP_DELAY);
        }

        if (CurrentPhase is DuelPhase.Battle)
        {
            await Main2PhaseAsync(STARTUP_DELAY);
        }

        CurrentPhase = DuelPhase.End;

        GetPhaseButton(CurrentTurnSide, CurrentPhase).ChangeStatus(false);

        await _popupPhase.ShowPhase(CurrentTurnSide, CurrentPhase);

        await Delay(delay);

        await ChangeTurnAsync();
        await DrawPhaseAsync();
    }
}
