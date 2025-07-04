using DMYAN.Scripts.GameManagerStack;
using Godot;
using static DMYAN.Scripts.Common.Constant;

namespace DMYAN.Scripts.Controls;

internal partial class PhaseButton : Button
{
    internal protected GameManager _gameManager;

    public override void _Ready()
    {
        var main = GetTree().Root.GetNode(MAIN_NODE);

        _gameManager = main.GetNode<GameManager>(nameof(GameManager));

        ButtonDown += OnButtonDown;
    }

    private void OnButtonDown() => AddThemeFontSizeOverride(FONT_SIZE_PROPERTY, BUTTON_DOWN_FONT_SIZE);

    internal void ChangeStatus(bool isEnable)
    {
        Disabled = !isEnable;

        if (isEnable)
        {
            AddThemeFontSizeOverride(FONT_SIZE_PROPERTY, BUTTON_UP_FONT_SIZE);
        }
        else
        {
            AddThemeFontSizeOverride(FONT_SIZE_PROPERTY, BUTTON_DOWN_FONT_SIZE);
        }
    }
}
