namespace DMYAN.Scripts.Controls;

internal partial class EpButton : PhaseButton
{
    public override void _Ready()
    {
        base._Ready();

        Pressed += OnPressed;
    }

    private async void OnPressed() => await _gameManager.EndPhaseAsync();
}
