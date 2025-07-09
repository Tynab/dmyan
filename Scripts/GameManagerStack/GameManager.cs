using DMYAN.Scripts.CardStack;
using DMYAN.Scripts.Common.Enum;
using DMYAN.Scripts.MainZoneStack;
using DMYAN.Scripts.ProfileStack;
using Godot;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YANLib;
using static DMYAN.Scripts.Common.Constant;
using static System.Threading.Tasks.Task;
using static YANLib.YANRandom;

namespace DMYAN.Scripts.GameManagerStack;

internal partial class GameManager : DMYANNode2D
{
    internal bool HasCardInMainZone(DuelSide side) => Cards.Any(x => x.DuelSide == side && x.Location is CardLocation.InBoard && x.Zone is CardZone.Main);

    internal int GraveyardCount(DuelSide side) => Cards.Count(x => x.DuelSide == side && x.Location == CardLocation.InBoard && x.Zone == CardZone.Graveyard && x.GraveyardIndex.HasValue);

    internal List<int> GetMainDeckIndices(DuelSide side) => [.. GetCardsInMainDeck(side).Select(static x => x.MainDeckIndex.Value)];

    #region Get Objects

    internal static DuelSide GetOpposite(DuelSide side) => side switch
    {
        DuelSide.Player => DuelSide.Opponent,
        DuelSide.Opponent => DuelSide.Player,
        _ => DuelSide.None
    };

    internal Profile GetProfile(DuelSide side) => side switch
    {
        DuelSide.Player => PlayerProfile,
        DuelSide.Opponent => OpponentProfile,
        _ => null
    };

    internal MainZone GetMainZone(DuelSide side) => side switch
    {
        DuelSide.Player => PlayerMainZone,
        DuelSide.Opponent => OpponentMainZone,
        _ => null
    };

    internal Graveyard GetGraveyard(DuelSide side) => side switch
    {
        DuelSide.Player => PlayerGraveyard,
        DuelSide.Opponent => OpponentGraveyard,
        _ => null
    };

    internal Vector2 GetAvatarPosition(DuelSide side) => side switch
    {
        DuelSide.Player => PlayerProfile.GetAvatarPosition(),
        DuelSide.Opponent => OpponentProfile.GetAvatarPosition(),
        _ => Vector2.Zero
    };

    #endregion

    #region Cards

    internal List<Card> GetCards(DuelSide side) => [.. Cards.Where(x => x.DuelSide == side)];

    internal List<Card> GetCardsInMainDeck(DuelSide side) => [.. Cards.Where(x => x.DuelSide == side && x.Location == CardLocation.InDeck && x.Zone == CardZone.MainDeck && x.MainDeckIndex.HasValue)];

    internal List<Card> GetCardsInExtraDeck(DuelSide side) => [.. Cards.Where(x => x.DuelSide == side && x.Location == CardLocation.InDeck && x.Zone == CardZone.ExtraDeck && x.ExtraDeckIndex.HasValue)];

    internal List<Card> GetCardsInHand(DuelSide side) => [.. Cards.Where(x => x.DuelSide == side && x.Location == CardLocation.InHand && x.HandIndex.HasValue)];

    internal List<Card> GetCardsInGraveyard(DuelSide side) => [.. Cards.Where(x => x.DuelSide == side && x.Location == CardLocation.InBoard && x.Zone == CardZone.Graveyard && x.GraveyardIndex.HasValue)];

    internal List<Card> GetCardInHandCanSummon(DuelSide side) => [.. Cards.Where(x => x.DuelSide == side && x.Location == CardLocation.InHand && x.HandIndex.HasValue && x.CanSummon)];

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

    internal List<Card> GetCardsInMainZone(DuelSide side) => [.. Cards.Where(x => x.DuelSide == side && x.Location == CardLocation.InBoard && x.Zone == CardZone.Main)];

    internal List<Card> GetFaceUpCardsInMainZone(DuelSide side) => [.. GetCardsInMainZone(side).Where(x => x.CardFace is CardFace.FaceUp)];

    internal List<Card> GetFaceDownCardsInMainZone(DuelSide side) => [.. GetCardsInMainZone(side).Where(x => x.CardFace is CardFace.FaceDown)];

    internal List<Card> GetDescendingAtkCardsInMainZone(DuelSide side) => [.. GetFaceUpCardsInMainZone(side).Where(x => x.CardPosition is CardPosition.Attack).OrderByDescending(x => x.ATK)];

    internal List<Card> GetDescendingDefCardsInMainZone(DuelSide side) => [.. GetFaceUpCardsInMainZone(side).Where(x => x.CardPosition is CardPosition.Defense).OrderByDescending(x => x.DEF)];

    internal Card GetHighestAtkCardInMainZone(DuelSide side) => GetDescendingAtkCardsInMainZone(side).FirstOrDefault();

    internal Card GetHighestDefCardInMainZone(DuelSide side) => GetDescendingDefCardsInMainZone(side).FirstOrDefault();

    internal Card GetRandomFaceDownCardInMainZone(DuelSide side)
    {
        var cards = GetFaceDownCardsInMainZone(side);

        return cards.IsNullEmpty() ? default : cards[GenerateRandom<int>(min: 0, max: cards.Count)];
    }

    #endregion

    #region Phases

    internal async Task DrawPhase(int delay = PHASE_CHANGE_DELAY)
    {
        CurrentPhase = DuelPhase.Draw;
        HasSummoned = false;

        GetPhaseButton(CurrentTurnSide, CurrentPhase).ChangeStatus(false);

        await _popupPhase.ShowPhase(CurrentTurnSide, CurrentPhase);

        if (CurrentTurnSide is DuelSide.Player)
        {
            await DrawStep(PlayerMainDeck, PlayerHand);
        }
        else
        {
            await DrawStep(OpponentMainDeck, OpponentHand);
        }

        await Delay(delay);

        await StandbyPhase();
    }

    internal async Task StandbyPhase(int delay = PHASE_CHANGE_DELAY)
    {
        CurrentPhase = DuelPhase.Standby;

        GetPhaseButton(CurrentTurnSide, CurrentPhase).ChangeStatus(false);

        await _popupPhase.ShowPhase(CurrentTurnSide, CurrentPhase);

        await Delay(delay);

        await Main1Phase();
    }

    internal async Task Main1Phase(int delay = PHASE_CHANGE_DELAY)
    {
        CurrentPhase = DuelPhase.Main1;

        GetPhaseButton(CurrentTurnSide, CurrentPhase).ChangeStatus(false);

        await _popupPhase.ShowPhase(CurrentTurnSide, CurrentPhase);

        CanSummonOrSet();

        if (CurrentTurnSide is DuelSide.Opponent)
        {
            await this.AiMainPhase();
        }

        await Delay(delay);
    }

    internal async Task BattlePhase(int delay = PHASE_CHANGE_DELAY)
    {
        CurrentPhase = DuelPhase.Battle;

        GetPhaseButton(CurrentTurnSide, CurrentPhase).ChangeStatus(false);

        await _popupPhase.ShowPhase(CurrentTurnSide, CurrentPhase);

        if (!IsFirstTurn)
        {
            Cards.ForEach(async x => await x.CanAttackCheck(CurrentTurnSide));
        }

        if (CurrentTurnSide is DuelSide.Opponent)
        {
            await this.AiBattlePhase();
        }

        await Delay(delay);
    }

    internal async Task Main2Phase(int delay = PHASE_CHANGE_DELAY)
    {
        CurrentPhase = DuelPhase.Main2;

        Cards.Where(x => x.DuelSide == CurrentTurnSide && x.Location is CardLocation.InBoard && x.Zone is CardZone.Main && x.CardFace is CardFace.FaceUp && x.CardPosition is CardPosition.Attack)
            .ToList()
            .ForEach(async x => await x.Sword.FadeOut());

        GetPhaseButton(CurrentTurnSide, CurrentPhase).ChangeStatus(false);

        await _popupPhase.ShowPhase(CurrentTurnSide, CurrentPhase);

        await Delay(delay);
    }

    internal async Task EndPhase(int delay = STARTUP_DELAY)
    {
        if (CurrentPhase is DuelPhase.Main1)
        {
            await BattlePhase(STARTUP_DELAY);
        }

        if (CurrentPhase is DuelPhase.Battle)
        {
            await Main2Phase(STARTUP_DELAY);
        }

        CurrentPhase = DuelPhase.End;

        GetPhaseButton(CurrentTurnSide, CurrentPhase).ChangeStatus(false);

        await _popupPhase.ShowPhase(CurrentTurnSide, CurrentPhase);

        await Delay(delay);

        await ChangeTurn();
        await DrawPhase();
    }

    #endregion
}
