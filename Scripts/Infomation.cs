using Godot;
using static DMYAN.Scripts.Constant;
using static Godot.ResourceLoader;

namespace DMYAN.Scripts;

public partial class Infomation : Node2D
{
    private RichTextLabel _name;
    private RichTextLabel _lp;
    private TextureProgressBar _lifePoint;
    private TextureProgressBar _timer;
    private TextureRect _avatar;

    public override void _Ready()
    {
        _name = GetNode<RichTextLabel>(INFO_NAME_NODE);
        _lp = GetNode<RichTextLabel>(INFO_LP_NODE);
        _lifePoint = GetNode<TextureProgressBar>(INFO_LIFE_POINT_NODE);
        _timer = GetNode<TextureProgressBar>(INFO_TIMER_NODE);
        _avatar = GetNode<TextureRect>(nameof(TextureRect));
    }

    public void Initialize(string name, float lp = 10_000)
    {
        _name.Text = name;
        _lp.Text = lp.ToString();
        _lifePoint.MaxValue = lp;
        _avatar.Texture = Load<Texture2D>($"res://Assets/Avatars/{name}.jpg");
    }
}
