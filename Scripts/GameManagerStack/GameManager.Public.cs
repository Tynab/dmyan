using DMYAN.Scripts.Common;
using Godot;
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
    internal async Task DrawPhaseAsync(int delay = PHASE_CHANGE_DELAY)
    {
        CurrentPhase = DuelPhase.Draw;
        HasSummoned = false;

        _dpButton.ChangeStatus(false);

        await _popupPhase.ShowPhase(PopupPhaseType.DP);

        if (CurrentTurnSide is DuelSide.Player)
        {
            await DrawAndPlaceCardAsync(PlayerDeck, PlayerHand);
        }
        else
        {
            await DrawAndPlaceCardAsync(OpponentDeck, OpponentHand);
        }

        await Delay(delay);

        await StandbyPhaseAsync();
    }

    internal async Task StandbyPhaseAsync(int delay = PHASE_CHANGE_DELAY)
    {
        CurrentPhase = DuelPhase.Standby;

        _spButton.ChangeStatus(false);

        await _popupPhase.ShowPhase(PopupPhaseType.SP);

        await Delay(delay);

        await Main1PhaseAsync();
    }

    internal async Task Main1PhaseAsync(int delay = PHASE_CHANGE_DELAY)
    {
        CurrentPhase = DuelPhase.Main1;

        _m1Button.ChangeStatus(false);

        await _popupPhase.ShowPhase(PopupPhaseType.M1);

        await Delay(delay);
    }

    internal async Task BattlePhaseAsync(int delay = PHASE_CHANGE_DELAY)
    {
        CurrentPhase = DuelPhase.Battle;

        _bpButton.ChangeStatus(false);

        await _popupPhase.ShowPhase(PopupPhaseType.BP);

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

        _m2Button.ChangeStatus(false);

        await _popupPhase.ShowPhase(PopupPhaseType.M2);

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

        _epButton.ChangeStatus(false);

        await _popupPhase.ShowPhase(PopupPhaseType.EP);

        await Delay(delay);

        await ChangeTurnAsync();
        await DrawPhaseAsync();
    }
}
