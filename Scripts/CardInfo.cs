using Godot;
using static DMYAN.Scripts.Constant;
using static Godot.ResourceLoader;

namespace DMYAN.Scripts;

public partial class CardInfo : Node2D
{
    public int CurrentSwap { get; set; } = DEFAULT_CARD_INFO_SWAP;

    public string TexturePath1 { get; set; } = CARD_BACK_ASSET_PATH;

    public string TexturePath2 { get; set; } = CARD_BACK_ASSET_PATH;

    private AnimationPlayer _animationPlayer;

    public override void _Ready() => _animationPlayer = GetNode<AnimationPlayer>(DEFAULT_ANIMATION_PLAYER_NODE);

    public void UpdateTexture()
    {
        GetNode<Sprite2D>(CARD_INFO_SWAP_1_NODE).Texture = Load<Texture2D>(TexturePath1);
        GetNode<Sprite2D>(CARD_INFO_SWAP_2_NODE).Texture = Load<Texture2D>(TexturePath2);
        AnimationSwap();
        CurrentSwap = CurrentSwap is 1 ? 2 : 1;
    }

    public void UpdateDescription(Card card)
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

            description += $"\nCông: {card.ATK.ViewPower()} / Thủ: {card.DEF.ViewPower()}";

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
