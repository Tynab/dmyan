using DMYAN.Scripts.Common.Enum;
using Godot;

namespace DMYAN.Scripts.MainZoneStack;

internal partial class MainZone : Node2D
{
    [Export]
    private DuelSide DuelSide { get; set; } = DuelSide.None;

    internal int CardsInZone { get; set; } = 0;

    internal bool HasCardCanAttack { get; set; } = false;
}
