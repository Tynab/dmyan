using DMYAN.Scripts.Common.Enum;
using Godot;
using System.Threading.Tasks;
using static DMYAN.Scripts.Common.Constant;
using static Godot.Mathf;
using static Godot.Tween.EaseType;
using static Godot.Tween.TransitionType;
using static Godot.Vector2;
using static System.Threading.Tasks.Task;

namespace DMYAN.Scripts.SwordStack;

internal partial class Sword : DMYANNode2D
{
    internal async Task DirectAttack(DuelSide side)
    {
        await AnimationAttack(_gameManager.GetAvatarPosition(side));

        await _gameManager.GetProfile(side).UpdateLifePoint(-_gameManager.CardAttacking.ATK.Value);

        _gameManager.CurrentStep = DuelStep.Attacked;
    }

    internal async Task AnimationAttack(Vector2 globalPosition)
    {
        _isSwordFollowingMouse = false;

        _sprite.LookAt(globalPosition);
        _sprite.Rotation += Pi / 2;

        var parent = GetParent().GetParent<DMYANNode2D>();
        var h = _sprite.Texture.GetHeight() * parent.Scale.Y / 2;
        var angleRadians = DegToRad(90 - _sprite.RotationDegrees);

        _ = _gameManager.CurrentTurnSide is DuelSide.Player
            ? GetTree()
                .CreateTween()
                .SetTrans(Circ)
                .SetEase(Out).
                TweenProperty(this, GLOBAL_POSITION_NODE_PATH, new Vector2(globalPosition.X - (float)(Cos(angleRadians) * h), globalPosition.Y + (float)(Sin(angleRadians) * h)), DEFAULT_ANIMATION_SPEED)
            : GetTree()
                .CreateTween()
                .SetTrans(Circ)
                .SetEase(Out)
                .TweenProperty(this, GLOBAL_POSITION_NODE_PATH, new Vector2(globalPosition.X + (float)(Cos(angleRadians) * h), globalPosition.Y - (float)(Sin(angleRadians) * h)), DEFAULT_ANIMATION_SPEED);

        await Delay(ATTACK_DELAY);

        await FadeOut();

        Position = Zero;

        _sprite.Rotation = _originalSwordRotation;
    }
}
