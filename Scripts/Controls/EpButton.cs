using static DMYAN.Scripts.Common.Constant;

namespace DMYAN.Scripts.Controls;

public partial class EpButton : PhaseButton
{
    private GameManager _gameManager;

    public override void _Ready()
    {
        _gameManager = GetNode<GameManager>($"../../{nameof(GameManager)}");
        ButtonDown += OnButtonDown;
        Pressed += OnPressed;
    }

    private void OnButtonDown() => AddThemeFontSizeOverride(FONT_SIZE_PROPERTY, BUTTON_DOWN_FONT_SIZE);

    private async void OnPressed() => await _gameManager.EndPhaseAsync();
}
