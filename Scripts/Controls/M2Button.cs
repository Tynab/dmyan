using DMYAN.Scripts.Common.Enum;

namespace DMYAN.Scripts.Controls;

internal partial class M2Button : PhaseButton
{
    public override void _Ready()
    {
        base._Ready();

        Pressed += OnPressed;
    }

    private void OnPressed()
    {
        if (!IsClicked && _gameManager.CurrentPhase is not DuelPhase.Draw and not DuelPhase.Standby)
        {
            IsClicked = true;
        }
    }
}
