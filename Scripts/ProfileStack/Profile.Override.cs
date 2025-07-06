using Godot;
using static DMYAN.Scripts.Common.Constant;

namespace DMYAN.Scripts.ProfileStack;

internal partial class Profile : Node2D
{
    public override void _Ready()
    {
        _name = GetNode<RichTextLabel>(INFO_NAME_NODE);
        _lifePoint = GetNode<RichTextLabel>(INFO_LIFE_POINT_NODE);
        _health = GetNode<TextureProgressBar>(INFO_HEALTH_NODE);
        _timer = GetNode<TextureProgressBar>(INFO_TIMER_NODE);
        _avatar = GetNode<TextureRect>(nameof(TextureRect));
    }
}
