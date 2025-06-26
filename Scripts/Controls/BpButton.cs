namespace DMYAN.Scripts.Controls;

internal partial class BpButton : PhaseButton
{
    public override void _Ready()
    {
        base._Ready();

        Pressed += OnPressed;
    }

    private async void OnPressed() => await _gameManager.BattlePhaseAsync();
}
