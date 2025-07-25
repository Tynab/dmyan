using DMYAN.Scripts.CardStack;
using DMYAN.Scripts.Common;
using DMYAN.Scripts.Common.Enum;
using Godot;
using YANLib;
using static DMYAN.Scripts.Common.Constant;
using static Godot.ResourceLoader;

namespace DMYAN.Scripts.CardInfoStack;

internal partial class CardInfo : DMYANNode2D
{
    private void UpdateTexture()
    {
        GetNode<Sprite2D>(CARD_INFO_SWAP_1_NODE).Texture = Load<Texture2D>(TexturePath1);
        GetNode<Sprite2D>(CARD_INFO_SWAP_2_NODE).Texture = Load<Texture2D>(TexturePath2);
    }

    private void UpdateDescription(Card card)
    {
        var description = string.Empty;

        if (card.Type is CardType.Monster)
        {
            description += $"Thuộc tính: {card.Attribute.VietTranslation().ToUpper()}\nChủng loài: {card.Race.VietTranslation()}";

            var strProperty = card.Property.VietTranslation();

            if (strProperty.IsNotNullWhiteSpace())
            {
                description += $"/{strProperty}";
            }

            description += "\nCấp: ";

            for (var i = 0; i < card.Level; i++)
            {
                description += "★";
            }

            description += $"\nCông: {card.BaseATK.ViewPower()} / Thủ: {card.BaseDEF.ViewPower()}";

            if (card.Description.IsNotNullWhiteSpace())
            {
                description += $"\nHiệu ứng:\n{card.Description}";
            }
        }
        else
        {
            var strProperty = card.Property.VietTranslation();

            if (strProperty.IsNotNullWhiteSpace())
            {
                description += $"Đặc tính: {strProperty}\n";
            }

            description += $"Hiệu ứng:\n{card.Description}";
        }

        GetNode<RichTextLabel>(CARD_INFO_HEADER_NODE).Text = $"{card.CardName.ToUpper()}\n[{card.Type.VietTranslation()}]";
        GetNode<RichTextLabel>(CARD_INFO_DETAIL_NODE).Text = description;
    }
}
