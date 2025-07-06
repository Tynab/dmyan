using DMYAN.Scripts.PowerSlotStack;
using Godot;

namespace DMYAN.Scripts.MainCardSlotStack;

internal partial class MainCardSlot : CardSlot
{
    [Export]
    internal PowerSlot PowerSlot { get; set; }
}
