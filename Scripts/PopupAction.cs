using Godot;
using static DMYAN.Scripts.Constant;
using static Godot.ResourceLoader;

namespace DMYAN.Scripts;

public partial class PopupAction : Sprite2D
{
    public void ShowSummonPopup()
    {
        Texture = Load<Texture2D>(SUMMON_POPUP_ASSET_PATH);
        Visible = true;
    }

    public void ShowSetPopup()
    {
        Texture = Load<Texture2D>(SET_POPUP_ASSET_PATH);
        Visible = true;
    }

    public void ShowActivePopup()
    {
        Texture = Load<Texture2D>(ACTIVE_POPUP_ASSET_PATH);
        Visible = true;
    }

    public void ShowAttackPopup()
    {
        Texture = Load<Texture2D>(ATK_POPUP_ASSET_PATH);
        Visible = true;
    }

    public void ShowDefensePopup()
    {
        Texture = Load<Texture2D>(DEF_POPUP_ASSET_PATH);
        Visible = true;
    }

    public void HidePopup() => Visible = false;
}
