using DMYAN.Scripts.Common.Enum;
using Godot;

namespace DMYAN.Scripts.CardStack;

internal partial class Card : Node2D
{
    private void OnAreaMouseEntered()
    {
        if (DuelSide is DuelSide.Player)
        {
            if (Location is CardLocation.InBoard && Zone is CardZone.Field or CardZone.Main or CardZone.STP)
            {
                _canView = true;
                _gameManager.CardInfo.BindingData(this);
            }
            else if (Location is CardLocation.InHand)
            {
                _canView = true;
                _gameManager.CardInfo.BindingData(this);

                HighlightOn();

                if (_gameManager.CurrentTurnSide is DuelSide.Player)
                {
                    if (CanActivate)
                    {
                        PopupAction.ShowAction(PopupActionType.Activate);

                        ActionType = CardActionType.Activate;
                    }
                    else if (CanSummon)
                    {
                        PopupAction.ShowAction(PopupActionType.Summon);

                        ActionType = CardActionType.Summon;
                    }
                }
            }
        }
    }

    private void OnAreaMouseExited()
    {
        if (_canView)
        {
            if (Location is CardLocation.InHand)
            {
                HighlightOff();
            }

            PopupAction.Hide();

            ActionType = CardActionType.None;
        }
    }
}
