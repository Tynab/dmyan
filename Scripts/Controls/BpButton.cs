namespace DMYAN.Scripts.Controls;

internal partial class BPButton : PhaseButton
{
    public override void _Ready()
    {
        base._Ready();

        Pressed += OnPressed;
    }

    private async void OnPressed() => await _gameManager.BattlePhaseAsync();
}
