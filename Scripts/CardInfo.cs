using Godot;
using System.Threading.Tasks;
using static DMYAN.Scripts.Constant;
using static Godot.AnimationMixer.SignalName;
using static Godot.Tween.EaseType;
using static Godot.Tween.TransitionType;
using static Godot.ResourceLoader;

namespace DMYAN.Scripts;

public partial class CardInfo : Node2D
{
    public int CurrentSwap { get; set; } = DEFAULT_CARD_INFO_SWAP;

    public string TexturePath1 { get; set; } = CARD_BACK_ASSET_PATH;

    public string TexturePath2 { get; set; } = CARD_BACK_ASSET_PATH;

    private AnimationPlayer _animationPlayer;

    public override void _Ready() => _animationPlayer = GetNodeOrNull<AnimationPlayer>(DEFAULT_ANIMATION_PLAYER_NODE);

    public void UpdateTexture()
    {
        GetNodeOrNull<Sprite2D>(CARD_INFO_SWAP_1_NODE).Texture = Load<Texture2D>(TexturePath1);
        GetNodeOrNull<Sprite2D>(CARD_INFO_SWAP_2_NODE).Texture = Load<Texture2D>(TexturePath2);
        AnimationDrawFlipAsync();
        CurrentSwap = CurrentSwap is 1 ? 2 : 1;
    }

    public void UpdateDescription(string name, CardType type, CardProperty property, MonsterAttribute attribute, MonsterRace race, int level, int atk, int def, string effect)
    {
        var description = string.Empty;

        if (type is CardType.Monster)
        {
            description += $"Thuộc tính: {attribute.VietTranslation().ToUpper()}\nChủng loài: {race.VietTranslation()}";

            var strProperty = property.VietTranslation();

            if (!string.IsNullOrWhiteSpace(strProperty))
            {
                description += $"/{strProperty}";
            }

            description += "\nCấp: ";

            for (var i = 0; i < level; i++)
            {
                description += "★";
            }

            description += $"\nCông: {atk.ViewPower()} / Thủ: {def.ViewPower()}";

            if (!string.IsNullOrWhiteSpace(effect))
            {
                description += $"\nHiệu ứng:\n{effect}";
            }
        }
        else
        {
            var strProperty = property.VietTranslation();

            if (!string.IsNullOrWhiteSpace(strProperty))
            {
                description += $"Đặc tính: {strProperty}\n";
            }

            description += $"Hiệu ứng:\n{effect}";
        }

        GetNodeOrNull<RichTextLabel>(CARD_INFO_HEADER_NODE).Text = $"{name.ToUpper()}\n[{type.VietTranslation()}]";
        GetNodeOrNull<RichTextLabel>(CARD_INFO_DETAIL_NODE).Text = description;
    }

    private void AnimationDrawFlipAsync() => _animationPlayer.Play(CurrentSwap is 1 ? CARD_SWAP_1_ANIMATION : CARD_SWAP_2_ANIMATION);
}
