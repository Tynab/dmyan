using DMYAN.Scripts.CardStack;
using Godot;
using System.Threading.Tasks;
using static DMYAN.Scripts.Common.Constant;
using static Godot.Colors;

namespace DMYAN.Scripts.PowerSlotStack;

internal partial class PowerSlot : DMYANNode2D
{
    public override void _Ready()
    {
        _atk = GetNode<RichTextLabel>(POWER_ATK_NODE);
        _def = GetNode<RichTextLabel>(POWER_DEF_NODE);
        _slash = GetNode<RichTextLabel>(POWER_SLASH_NODE);
        _slash.Modulate = Gray;
    }

    internal async Task ShowPower(Card card, bool isAtk)
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

        await FadeIn();
    }

    internal async Task HidePower()
    {
        await FadeOut();

        _atk.Text = string.Empty;
        _def.Text = string.Empty;
        _slash.Text = string.Empty;
    }
}
