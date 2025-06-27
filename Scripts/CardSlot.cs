using DMYAN.Scripts.Common.Enum;
using Godot;

namespace DMYAN.Scripts;

internal partial class CardSlot : Node2D
{
    [Export]
    internal protected DuelSide DuelSide { get; set; } = DuelSide.None;

    internal bool HasCardInSlot { get; set; } = false;
}
