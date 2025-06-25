using Godot;
using static DMYAN.Scripts.Constant;
using static Godot.Colors;

namespace DMYAN.Scripts;

public partial class PowerSlot : Node2D
{
    [Export]
    public DuelSide DuelSide { get; set; } = DuelSide.None;

    public bool HasCardInMainZone { get; set; } = false;

    public CardFace CardFaceInMainZone { get; set; } = CardFace.None;

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

    public void ShowPower(Card card, bool isAtk)
    {
        _atk.Text = card.ATK.ToString();
        _def.Text = card.DEF.ToString();

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
