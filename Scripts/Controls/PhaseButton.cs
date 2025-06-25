using Godot;
using static DMYAN.Scripts.Constant;

namespace DMYAN.Scripts.Controls;

public partial class PhaseButton : Button
{
    public void ChangeStatus(bool isEnable)
    {
        Disabled = !isEnable;
        AddThemeFontSizeOverride(FONT_SIZE_PROPERTY, BUTTON_UP_FONT_SIZE);
    }
}
