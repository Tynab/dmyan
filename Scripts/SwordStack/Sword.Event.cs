using DMYAN.Scripts.CardStack;
using DMYAN.Scripts.Common.Enum;
using DMYAN.Scripts.GameManagerStack;
using Godot;
using static Godot.MouseButton;

namespace DMYAN.Scripts.SwordStack;

internal partial class Sword : DMYANNode2D
{
    private async void OnAreaInputEvent(Node viewport, InputEvent @event, long shapeIdx)
    {
        if (@event is InputEventMouseButton mouseButtonEvent && mouseButtonEvent.Pressed && mouseButtonEvent.ButtonIndex is Left)
        {
            _gameManager.CurrentStep = DuelStep.Attacking;
            _gameManager.CardAttacking = GetParent<Card>();

            var opposite = GameManager.GetOpposite(_gameManager.CurrentTurnSide);

            if (_gameManager.HasCardInMainZone(opposite))
            {
                _isSwordFollowingMouse = true;
            }
            else
            {
                await DirectAttack(opposite);
            }
        }
    }
}
