using Godot;

namespace DMYAN.Scripts;

public partial class STPCardSlot : CardSlot
{
    [Export]
    public int Index { get; set; } = 0;

    public CardFace CardFaceInSlot { get; set; } = CardFace.None;
}
