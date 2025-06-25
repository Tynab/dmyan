using static DMYAN.Scripts.Common.Constant;

namespace DMYAN.Scripts.Controls;

public partial class M2Button : PhaseButton
{
    private GameManager _gameManager;

    public override void _Ready()
    {
        _gameManager = GetNode<GameManager>($"../../{nameof(GameManager)}");
        ButtonDown += OnButtonDown;
    }

    private void OnButtonDown() => AddThemeFontSizeOverride(FONT_SIZE_PROPERTY, BUTTON_DOWN_FONT_SIZE);
}
