using Godot;
using System;
using System.Drawing;
using System.Threading.Tasks;
using static DMYAN.Scripts.Common.Constant;
using static Godot.Mathf;
using static Godot.MouseButton;
using static Godot.Tween.EaseType;
using static Godot.Tween.TransitionType;

namespace DMYAN.Scripts;

public partial class Eye : Node2D
{
	private AnimationPlayer _animationPlayer;

	public override void _Ready() => _animationPlayer = GetNode<AnimationPlayer>(DEFAULT_ANIMATION_PLAYER_NODE);

	internal void AnimationShow() => _animationPlayer.Play(EYE_SHOW_ANIMATION);

	internal void AnimationHide() => _animationPlayer.Play(EYE_HIDE_ANIMATION);
}
