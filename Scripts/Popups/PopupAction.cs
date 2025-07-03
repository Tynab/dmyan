using Godot;
using static DMYAN.Scripts.Common.Constant;
using static Godot.ResourceLoader;

namespace DMYAN.Scripts.Popups;

internal partial class PopupAction : Sprite2D
{
    internal void ShowAction(PopupActionType type)
    {
        var path = type switch
        {
            PopupActionType.Summon => SUMMON_POPUP_ASSET_PATH,
            PopupActionType.Set => SET_POPUP_ASSET_PATH,
            PopupActionType.Activate => ACTIVATE_POPUP_ASSET_PATH,
            PopupActionType.Attack => ATK_POPUP_ASSET_PATH,
            PopupActionType.Defense => DEF_POPUP_ASSET_PATH,
            _ => string.Empty
        };

        if (string.IsNullOrWhiteSpace(path))
        {
            return;
        }

        Texture = Load<Texture2D>(path);

        Show();
    }
}
