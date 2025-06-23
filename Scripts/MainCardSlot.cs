using Godot;

namespace DMYAN.Scripts;

public partial class MainCardSlot : CardSlot
{
    [Export]
    public int Index { get; set; } = 0;

    public CardFace CardFaceInSlot { get; set; } = CardFace.None;

    public int CardsInSlot { get; set; } = 0;
}
