using DMYAN.Scripts.CardStack;
using DMYAN.Scripts.Common.Enum;
using Godot;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static DMYAN.Scripts.Common.Constant;
using static System.Threading.Tasks.Task;

namespace DMYAN.Scripts.GameManagerStack;

internal partial class GameManager : Node2D
{
    internal List<Card> GetCardsInMainDeck(DuelSide side) => [.. Cards.Where(x => x.DuelSide == side && x.Location == CardLocation.InDeck && x.Zone == CardZone.MainDeck && x.MainDeckIndex.HasValue)];

    internal List<int> GetMainDeckIndices(DuelSide side) => [.. GetCardsInMainDeck(side).Select(x => x.MainDeckIndex.Value)];

    internal List<Card> GetCardsInExtraDeck(DuelSide side) => [.. Cards.Where(x => x.DuelSide == side && x.Location == CardLocation.InDeck && x.Zone == CardZone.ExtraDeck && x.ExtraDeckIndex.HasValue)];

    internal List<int> GetExtraDeckIndices(DuelSide side) => [.. GetCardsInExtraDeck(side).Select(x => x.ExtraDeckIndex.Value)];

    internal List<Card> GetCardsInHand(DuelSide side) => [.. Cards.Where(x => x.DuelSide == side && x.Location == CardLocation.InHand && x.HandIndex.HasValue)];

    internal List<int> GetHandIndices(DuelSide side) => [.. GetCardsInHand(side).Select(x => x.HandIndex.Value)];

    internal async Task DrawPhaseAsync(int delay = PHASE_CHANGE_DELAY)
    {
        CurrentPhase = DuelPhase.Draw;
        HasSummoned = false;

        GetPhaseButton(CurrentTurnSide, CurrentPhase).ChangeStatus(false);

        await _popupPhase.ShowPhase(CurrentTurnSide, CurrentPhase);

        if (CurrentTurnSide is DuelSide.Player)
        {
            await DrawAndPlaceCardAsync(PlayerMainDeck, PlayerHand);
        }
        else
        {
            await DrawAndPlaceCardAsync(OpponentMainDeck, OpponentHand);
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

        await Delay(delay);

        Cards.Where(x => x.DuelSide == CurrentTurnSide && x.Location is CardLocation.InHand).ToList().ForEach(x => x.CanSummonCheck());
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
