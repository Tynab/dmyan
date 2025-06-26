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
        if (_gameManager.AttackMode)
        {
            return;
        }
    }
}
