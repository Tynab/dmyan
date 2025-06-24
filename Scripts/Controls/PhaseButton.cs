using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DMYAN.Scripts.Constant;

namespace DMYAN.Scripts.Controls;

public partial class PhaseButton : Button
{
    public void Enable()
    {
        Disabled = false;
        AddThemeFontSizeOverride(FONT_SIZE_PROPERTY, BUTTON_UP_FONT_SIZE);
    }

    public void Disable()
    {
        Disabled = true;
        AddThemeFontSizeOverride(FONT_SIZE_PROPERTY, BUTTON_DOWN_FONT_SIZE);
    }
}
