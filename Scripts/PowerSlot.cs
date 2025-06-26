using DMYAN.Scripts.Common;
using Godot;
using static DMYAN.Scripts.Common.Constant;
using static Godot.Colors;

namespace DMYAN.Scripts;

internal partial class PowerSlot : Node2D
{
    [Export]
    private DuelSide DuelSide { get; set; } = DuelSide.None;

    internal bool HasCardInMainZone { get; set; } = false;

    internal CardFace CardFaceInMainZone { get; set; } = CardFace.None;

    private RichTextLabel _atk;
    private RichTextLabel _def;
    private RichTextLabel _slash;

    public override void _Ready()
    {
        _atk = GetNode<RichTextLabel>(POWER_ATK_NODE);
        _def = GetNode<RichTextLabel>(POWER_DEF_NODE);
        _slash = GetNode<RichTextLabel>(POWER_SLASH_NODE);
        _slash.Modulate = Gray;
    }

    internal void ShowPower(Card card, bool isAtk)
    {
        _atk.Text = card.BaseATK.ToString();
        _def.Text = card.BaseDEF.ToString();

        if (isAtk)
        {
            _atk.Modulate = White;
            _def.Modulate = Gray;
        }
        else
        {
            _atk.Modulate = Gray;
            _def.Modulate = White;
        }

        Show();
    }
}
