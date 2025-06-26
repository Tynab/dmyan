using DMYAN.Scripts.Common;
using Godot;
using static DMYAN.Scripts.Common.Constant;
using static Godot.ResourceLoader;

namespace DMYAN.Scripts;

internal partial class CardInfo : Node2D
{
    internal int CurrentSwap { get; set; } = DEFAULT_CARD_INFO_SWAP;

    internal string TexturePath1 { get; set; } = CARD_BACK_ASSET_PATH;

    internal string TexturePath2 { get; set; } = CARD_BACK_ASSET_PATH;

    private AnimationPlayer _animationPlayer;

    public override void _Ready() => _animationPlayer = GetNode<AnimationPlayer>(DEFAULT_ANIMATION_PLAYER_NODE);

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

            if (!string.IsNullOrWhiteSpace(strProperty))
            {
                description += $"/{strProperty}";
            }

            description += "\nCấp: ";

            for (var i = 0; i < card.Level; i++)
            {
                description += "★";
            }

            description += $"\nCông: {card.BaseATK.ViewPower()} / Thủ: {card.BaseDEF.ViewPower()}";

            if (!string.IsNullOrWhiteSpace(card.Description))
            {
                description += $"\nHiệu ứng:\n{card.Description}";
            }
        }
        else
        {
            var strProperty = card.Property.VietTranslation();

            if (!string.IsNullOrWhiteSpace(strProperty))
            {
                description += $"Đặc tính: {strProperty}\n";
            }

            description += $"Hiệu ứng:\n{card.Description}";
        }

        GetNode<RichTextLabel>(CARD_INFO_HEADER_NODE).Text = $"{card.CardName.ToUpper()}\n[{card.Type.VietTranslation()}]";
        GetNode<RichTextLabel>(CARD_INFO_DETAIL_NODE).Text = description;
    }

    private void AnimationSwap() => _animationPlayer.Play(CurrentSwap is 1 ? CARD_SWAP_1_ANIMATION : CARD_SWAP_2_ANIMATION);
}
