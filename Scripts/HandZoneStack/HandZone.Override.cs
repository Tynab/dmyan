using DMYAN.Scripts.GameManagerStack;
using Godot;
using static DMYAN.Scripts.Common.Constant;

namespace DMYAN.Scripts.HandZoneStack;

internal partial class HandZone : Node2D
{
    public override void _Ready()
    {
        var main = GetTree().Root.GetNode(MAIN_NODE);

        _gameManager = main.GetNode<GameManager>(nameof(GameManager));
    }
}
