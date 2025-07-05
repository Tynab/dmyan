using DMYAN.Scripts.Common.Enum;

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
        if (!IsClicked && _gameManager.CurrentPhase is not DuelPhase.Draw and not DuelPhase.Standby)
        {
            IsClicked = true;

            if (_gameManager.AttackMode)
            {
                return;
            }

            await _gameManager.EndPhaseAsync();
        }
    }
}
