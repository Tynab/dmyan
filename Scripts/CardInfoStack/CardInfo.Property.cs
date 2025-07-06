using Godot;
using static DMYAN.Scripts.Common.Constant;

namespace DMYAN.Scripts.CardInfoStack;

internal partial class CardInfo : Node2D
{
    internal int CurrentSwap { get; set; } = DEFAULT_CARD_INFO_SWAP;

    internal string TexturePath1 { get; set; } = CARD_BACK_ASSET_PATH;

    internal string TexturePath2 { get; set; } = CARD_BACK_ASSET_PATH;
}
