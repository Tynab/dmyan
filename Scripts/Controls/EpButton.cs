namespace DMYAN.Scripts.Controls;

internal partial class EPButton : PhaseButton
{
    public override void _Ready()
    {
        base._Ready();

        Pressed += OnPressed;
    }

    private async void OnPressed()
    {
        if (_gameManager.AttackMode)
        {
            return;
        }

        await _gameManager.EndPhaseAsync();
    }
}
