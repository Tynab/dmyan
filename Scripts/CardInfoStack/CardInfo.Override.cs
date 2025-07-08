using Godot;

namespace DMYAN.Scripts.CardInfoStack;

internal partial class CardInfo : DMYANNode2D
{
    public override void _Ready() => _animationPlayer = GetNode<AnimationPlayer>(nameof(AnimationPlayer));
}
