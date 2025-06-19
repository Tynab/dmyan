using Godot;

namespace DMYAN.Scripts;

public partial class STPCardSlotV : Node2D
{
	#region Properties

	public bool CardInSlot { get; set; } = false;

	public string Type { get; private set; } = "STP";

	#endregion
}
