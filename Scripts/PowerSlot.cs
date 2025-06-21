using Godot;
using static DMYAN.Scripts.Constant;

namespace DMYAN.Scripts;

public partial class PowerSlot : Node2D
{
    [Export]
    public int Index { get; set; } = 0;

    [Export]
    public DuelSide DuelSide { get; set; } = DuelSide.None;

    public bool HasCardInMainZone { get; set; } = false;

    public CardFace CardFaceInMainZone { get; set; } = CardFace.None;

    private RichTextLabel _atk;
    private RichTextLabel _def;
    private RichTextLabel _slash;

    public override void _Ready()
    {
        _atk = GetNodeOrNull<RichTextLabel>(ATK_NODE);
        _def = GetNodeOrNull<RichTextLabel>(DEF_NODE);
        _slash = GetNodeOrNull<RichTextLabel>(SLASH_NODE);
    }
}
