using Godot;

namespace DMYAN.Scripts.ProfileStack;

internal partial class Profile : DMYANNode2D
{
    private RichTextLabel _name;
    private RichTextLabel _lifePoint;
    private TextureProgressBar _health;
    private TextureProgressBar _timer;
    private TextureRect _avatar;
}
