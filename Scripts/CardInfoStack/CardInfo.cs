using DMYAN.Scripts.CardStack;
using DMYAN.Scripts.Common;
using static DMYAN.Scripts.Common.Constant;

namespace DMYAN.Scripts.CardInfoStack;

internal partial class CardInfo : DMYANNode2D
{
    internal void BindingData(Card card)
    {
        if (CurrentSwap is DEFAULT_CARD_INFO_SWAP)
        {
            TexturePath2 = card.Code.GetCardAssetPathByCode();
        }
        else
        {
            TexturePath1 = card.Code.GetCardAssetPathByCode();
        }

        UpdateTexture();
        AnimationSwap();
        UpdateDescription(card);

        CurrentSwap = CurrentSwap is 1 ? 2 : 1;
    }

    #region Animation

    internal void AnimationSwap() => _animationPlayer.Play(CurrentSwap is 1 ? CARD_SWAP_1_ANIMATION : CARD_SWAP_2_ANIMATION);

    #endregion
}
