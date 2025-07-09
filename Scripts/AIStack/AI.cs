using DMYAN.Scripts.Common.Enum;
using DMYAN.Scripts.GameManagerStack;
using System.Linq;
using System.Threading.Tasks;
using YANLib;
using static DMYAN.Scripts.Common.Constant;
using static System.Threading.Tasks.Task;

internal static partial class AI
{
    internal static async Task AiMainPhase(this GameManager gameManager)
    {
        var higherAtkCard = gameManager.GetHigherAtkCardInHand(DuelSide.Opponent);

        if (higherAtkCard.IsNull())
        {
            if (gameManager.IsFirstTurn)
            {
                var highestDefCard = gameManager.GetHighestDefCardInHand(DuelSide.Opponent);

                if (highestDefCard.IsNotNull())
                {
                    if (highestDefCard.DEF > highestDefCard.ATK)
                    {
                        await gameManager.OpponentMainZone.SummonSetCard(highestDefCard);
                    }
                    else
                    {
                        await gameManager.OpponentMainZone.SummonCard(highestDefCard);
                    }
                }
            }
            else
            {
                var highestDefCard = gameManager.GetHighestDefCardInHand(DuelSide.Opponent);

                if (highestDefCard.IsNotNull())
                {
                    await gameManager.OpponentMainZone.SummonSetCard(highestDefCard);
                }
            }
        }
        else
        {
            await gameManager.OpponentMainZone.SummonCard(higherAtkCard);
        }

        await gameManager.BattlePhase();
    }

    internal static async Task AiBattlePhase(this GameManager gameManager)
    {
        await Delay(PHASE_CHANGE_DELAY);

        var cards = gameManager.GetDescendingAtkCardsInMainZone(DuelSide.Opponent).Where(x => x.CanAttack).ToList();

        foreach (var card in cards)
        {
            gameManager.CardAttacking = card;

            var opponentCards = gameManager.GetCardsInMainZone(DuelSide.Player);

            if (opponentCards.IsNullEmpty())
            {
                await card.Sword.DirectAttack(DuelSide.Player);

                continue;
            }

            var opponentFaceupAttackables = gameManager.GetDescendingAtkCardsInMainZone(DuelSide.Player).Where(x => x.ATK <= card.ATK).ToList();

            foreach (var target in opponentFaceupAttackables)
            {
                if (!cards.Where(x => x != card).Any(x => x.ATK > target.ATK))
                {
                    await gameManager.AttackStep(target);

                    goto NextAttacker;
                }
            }

            var opponentFaceupDefensives = gameManager.GetDescendingDefCardsInMainZone(DuelSide.Player).Where(x => x.DEF < card.ATK).ToList();

            foreach (var target in opponentFaceupDefensives)
            {
                if (!cards.Where(x => x != card).Any(x => x.ATK > target.DEF))
                {
                    await gameManager.AttackStep(target);

                    goto NextAttacker;
                }
            }

            if (gameManager.GetFaceDownCardsInMainZone(DuelSide.Player).IsNotNullEmpty())
            {
                await gameManager.AttackStep(gameManager.GetRandomFaceDownCardInMainZone(DuelSide.Player));

                goto NextAttacker;
            }

            var highestDef = gameManager.GetHighestDefCardInMainZone(DuelSide.Player);

            if (highestDef.IsNotNull())
            {
                await gameManager.AttackStep(highestDef);

                goto NextAttacker;
            }

            var highestAtk = gameManager.GetHighestAtkCardInMainZone(DuelSide.Player);

            if (highestAtk.IsNotNull())
            {
                await gameManager.AttackStep(highestAtk);

                goto NextAttacker;
            }

            break;

        NextAttacker:
            continue;
        }

        await gameManager.EndPhase();
    }
}
