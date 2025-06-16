using Godot;

namespace DMYAN.Scripts;

public partial class Card : Node2D
{
    [Signal]
    public delegate void HoveredEventHandler(Card card);

    [Signal]
    public delegate void UnhoveredEventHandler(Card card);

    public override void _Ready() => GetParent().Call("ConnectCardSignals", this);

    public Vector2 StartingPosition { get; set; }

    private void OnArea2DMouseEntered() => EmitSignal(SignalName.Hovered, this);

    private void OnArea2DMouseExited() => EmitSignal(SignalName.Unhovered, this);
}
