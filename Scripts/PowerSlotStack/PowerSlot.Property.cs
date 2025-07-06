using DMYAN.Scripts.Common.Enum;
using Godot;

namespace DMYAN.Scripts.PowerSlotStack;

internal partial class PowerSlot : Node2D
{
    [Export]
    private DuelSide DuelSide { get; set; } = DuelSide.None;

    internal bool HasCardInMainZone { get; set; } = false;

    internal CardFace CardFaceInMainZone { get; set; } = CardFace.None;
}
