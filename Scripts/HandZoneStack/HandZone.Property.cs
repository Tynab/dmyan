using DMYAN.Scripts.Common.Enum;
using Godot;

namespace DMYAN.Scripts.HandZoneStack;

internal partial class HandZone : Node2D
{
    [Export]
    private DuelSide DuelSide { get; set; } = DuelSide.None;
}
