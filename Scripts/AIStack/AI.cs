using DMYAN.Scripts.Common.Enum;
using DMYAN.Scripts.GameManagerStack;
using System.Threading.Tasks;
using YANLib;

internal static partial class AI
{
    internal static async Task OpponentSummonOrSetCard(this GameManager gameManager)
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
                        gameManager.OpponentMainZone.SummonSetCard(highestDefCard);
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
                    gameManager.OpponentMainZone.SummonSetCard(highestDefCard);
                }
            }
        }
        else
        {
            await gameManager.OpponentMainZone.SummonCard(higherAtkCard);
        }

        await gameManager.EndPhaseAsync();
    }
}
