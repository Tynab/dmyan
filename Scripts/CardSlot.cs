using DMYAN.Scripts.Common.Enum;
using Godot;
using static DMYAN.Scripts.Common.Constant;

namespace DMYAN.Scripts;

internal partial class CardSlot : DMYANNode2D
{
    [Export]
    internal protected DuelSide DuelSide { get; set; } = DuelSide.None;

    internal bool HasCardInSlot { get; set; } = false;

    private RichTextLabel _richTextLabel;
    private AnimationPlayer _animationPlayer;

    public override void _Ready()
    {
        _richTextLabel = GetNode<RichTextLabel>(DAMAGE_NODE);
        _animationPlayer = GetNode<AnimationPlayer>(nameof(AnimationPlayer));
    }

    internal void AnimationShowDamage(int lpUpdate)
    {
        _richTextLabel.Text = lpUpdate.ToString();

        _animationPlayer.Play(SHOW_DAMAGE_ANIMATION);
    }
}
