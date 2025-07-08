using DMYAN.Scripts.Common.Enum;

namespace DMYAN.Scripts.CardStack;

internal partial class Card : DMYANNode2D
{
    private void OnAreaMouseEntered()
    {
        if (CanView)
        {
            _gameManager.CardInfo.BindingData(this);

            if (Location is CardLocation.InHand)
            {
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
        if (CanView)
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
