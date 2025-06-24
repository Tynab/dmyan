using Godot;
using static DMYAN.Scripts.Constant;
using static Godot.ResourceLoader;

namespace DMYAN.Scripts.Popups;

public partial class PopupAction : Sprite2D
{
    public void ShowSummonPopup()
    {
        Texture = Load<Texture2D>(SUMMON_POPUP_ASSET_PATH);
        Show();
    }

    public void ShowSetPopup()
    {
        Texture = Load<Texture2D>(SET_POPUP_ASSET_PATH);
        Show();
    }

    public void ShowActivePopup()
    {
        Texture = Load<Texture2D>(ACTIVE_POPUP_ASSET_PATH);
        Show();
    }

    public void ShowAttackPopup()
    {
        Texture = Load<Texture2D>(ATK_POPUP_ASSET_PATH);
        Show();
    }

    public void ShowDefensePopup()
    {
        Texture = Load<Texture2D>(DEF_POPUP_ASSET_PATH);
        Show();
    }
}
