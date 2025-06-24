using Godot;
using System;
using System.Threading.Tasks;

namespace DMYAN.Scripts.Controls;

public partial class EpButton : Button
{
    private GameManager _gameManager;

    public override void _Ready()
    {
        _gameManager = GetNode<GameManager>($"../../{nameof(GameManager)}");
        ButtonDown += OnButtonDown;
        Pressed += OnPressed;
    }

    private void OnButtonDown() => AddThemeFontSizeOverride("font_size", 35);

    private async void OnPressed() => await _gameManager.EndPhaseAsync();
}
