using DMYAN.Scripts.Common.Enum;
using DMYAN.Scripts.GameManagerStack;
using System.Threading.Tasks;
using YANLib;

internal static partial class AI
{
    internal static async Task OpponentSummonOrSetCard(this GameManager gameManager)
    {
        var higherAtkCardInHand = gameManager.GetHigherAtkCardInHand(DuelSide.Opponent);

        if (higherAtkCardInHand.IsNull())
        {
            if (gameManager.IsFirstTurn)
            {
                var highestDefCardInHand = gameManager.GetHighestDefCardInHand(DuelSide.Opponent);

                if (highestDefCardInHand.IsNotNull())
                {
                    if (highestDefCardInHand.DEF > highestDefCardInHand.ATK)
                    {
                        gameManager.OpponentMainZone.SummonSetCard(highestDefCardInHand);
                    }
                    else
                    {
                        await gameManager.OpponentMainZone.SummonCard(highestDefCardInHand);
                    }
                }
            }
            else
            {
                var highestDefCardInHand = gameManager.GetHighestDefCardInHand(DuelSide.Opponent);

                if (highestDefCardInHand.IsNotNull())
                {
                    gameManager.OpponentMainZone.SummonSetCard(highestDefCardInHand);
                }
            }
        }
        else
        {
            await gameManager.OpponentMainZone.SummonCard(higherAtkCardInHand);
        }

        await gameManager.EndPhaseAsync();
    }
}
