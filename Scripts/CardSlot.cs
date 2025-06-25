using DMYAN.Scripts.Common;
using Godot;

namespace DMYAN.Scripts;

public partial class CardSlot : Node2D
{
    [Export]
    public DuelSide DuelSide { get; set; } = DuelSide.None;

    public bool HasCardInSlot { get; set; } = false;
}
