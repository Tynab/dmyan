using Godot;
using System;
using System.Dynamic;

namespace DMYAN.Scripts;

public partial class CardInfo : Node2D
{
    public int CurrentSwap { get; set; } = 1;

    public string TexturePath1 { get; set; } = "res://Assets/card_back.jpg";

    public string TexturePath2 { get; set; } = "res://Assets/card_back.jpg";

    private AnimationPlayer _animationPlayer;

    public override void _Ready() => _animationPlayer = GetNodeOrNull<AnimationPlayer>("AnimationPlayer");

    public void UpdateTexture()
    {
        var cardSwap1 = GetNodeOrNull<Sprite2D>("CardSwap1");
        var cardSwap2 = GetNodeOrNull<Sprite2D>("CardSwap2");

        cardSwap1.Texture = ResourceLoader.Load<Texture2D>(TexturePath1);
        cardSwap2.Texture = ResourceLoader.Load<Texture2D>(TexturePath2);
        _animationPlayer.Play(CurrentSwap is 1 ? "card_swap_1" : "card_swap_2");
        CurrentSwap = CurrentSwap is 1 ? 2 : 1;
    }

    public void UpdateDescription(string name, CardType type, CardProperty property, MonsterAttribute attribute, MonsterRace race, int level, int atk, int def, string effect)
    {
        var description = string.Empty;

        if (type is CardType.Monster)
        {
            description += $"{attribute.ToString().ToUpper()}\n{race}";

            if (property is not CardProperty.None and not CardProperty.Normal)
            {
                description += $"/{property}";
            }

            description += "\n";

            for (var i = 0; i < level; i++)
            {
                description += "★";
            }

            description += $"\nATK {atk} / DEF {def}";

            if (!string.IsNullOrWhiteSpace(effect))
            {
                description += $"\n{effect}";
            }
        }
        else
        {
            if (property is not CardProperty.None and not CardProperty.Normal)
            {
                description += $"{property}\n";
            }

            description += $"{effect}";
        }

        GetNodeOrNull<RichTextLabel>("Header").Text = $"{name.ToUpper()}\n[{type}]";
        GetNodeOrNull<RichTextLabel>("Detail").Text = description;
    }
}
