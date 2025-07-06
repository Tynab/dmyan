using Godot;
using YANLib;
using static Godot.ResourceLoader;

namespace DMYAN.Scripts.ProfileStack;

internal partial class Profile : Node2D
{
    internal void Init(string name, float lp = 2_000)
    {
        _name.Text = name;
        _lifePoint.Text = lp.ToString();
        _health.MaxValue = lp;
        _avatar.Texture = Load<Texture2D>($"res://Assets/Avatars/{name}.jpg");
    }

    internal Vector2 GetAvatarPosition()
    {
        var position = _avatar.GlobalPosition;

        return new Vector2(position.X + _avatar.Size.X / 2, position.Y + _avatar.Size.Y / 2);
    }

    internal void UpdateLifePoint(int lpUpdate)
    {
        var lp = _lifePoint.Text.Parse<int>() + lpUpdate;

        _lifePoint.Text = lp.ToString();
        _health.Value = lp;
    }
}
