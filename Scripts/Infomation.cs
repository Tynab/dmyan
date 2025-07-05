using Godot;
using static DMYAN.Scripts.Common.Constant;
using static Godot.ResourceLoader;

namespace DMYAN.Scripts;

internal partial class Infomation : Node2D
{
    private RichTextLabel _name;
    private RichTextLabel _lifePoint;
    private TextureProgressBar _health;
    private TextureProgressBar _timer;
    private TextureRect _avatar;

    public override void _Ready()
    {
        _name = GetNode<RichTextLabel>(INFO_NAME_NODE);
        _lifePoint = GetNode<RichTextLabel>(INFO_LIFE_POINT_NODE);
        _health = GetNode<TextureProgressBar>(INFO_HEALTH_NODE);
        _timer = GetNode<TextureProgressBar>(INFO_TIMER_NODE);
        _avatar = GetNode<TextureRect>(nameof(TextureRect));
    }

    internal void Init(string name, float lp = 10_000)
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
}
