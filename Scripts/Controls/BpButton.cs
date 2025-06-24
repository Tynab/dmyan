using Godot;
using System;

namespace DMYAN.Scripts.Controls;

public partial class BpButton : PhaseButton
{
    private GameManager _gameManager;

    public override void _Ready()
    {
        _gameManager = GetNode<GameManager>($"../../{nameof(GameManager)}");
    }

    private void OnPressed()
    {
        //_gameManager.Ba
    }
}
