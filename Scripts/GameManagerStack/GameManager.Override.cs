using DMYAN.Scripts.Common;
using DMYAN.Scripts.Common.Enum;
using DMYAN.Scripts.Controls;
using DMYAN.Scripts.Popups;
using Godot;
using static DMYAN.Scripts.Common.CardDatabase;
using static DMYAN.Scripts.Common.Constant;
using static Godot.MouseButton;
using static System.Threading.Tasks.Task;

namespace DMYAN.Scripts.GameManagerStack;

internal partial class GameManager : Node2D
{
    public override async void _Ready()
    {
        LoadData();

        var parent = GetParent();
        //var board = parent.GetNode<Node2D>(BOARD_NODE);
        //var playerField = board.GetNode<Node2D>(FIELD_PLAYER_NODE);
        //var opponentField = board.GetNode<Node2D>(FIELD_OPPONENT_NODE);

        //_playerInfo = parent.GetNode<Infomation>(INFO_PLAYER_NODE);
        //_opponentInfo = parent.GetNode<Infomation>(INFO_OPPONENT_NODE);
        _popupPhase = parent.GetNode<PopupPhase>(nameof(PopupPhase));
        _control = parent.GetNode<Control>(nameof(Control));
        _playerControl = _control.GetNode<Node>("PlayerControl");
        _opponentControl = _control.GetNode<Node>("OpponentControl");

        _playerDpButton = _playerControl.GetNode<DPButton>(DP_BUTTON_NODE);
        _playerSpButton = _playerControl.GetNode<SPButton>(SP_BUTTON_NODE);
        _playerM1Button = _playerControl.GetNode<M1Button>(M1_BUTTON_NODE);
        _playerBpButton = _playerControl.GetNode<BPButton>(BP_BUTTON_NODE);
        _playerM2Button = _playerControl.GetNode<M2Button>(M2_BUTTON_NODE);
        _playerEpButton = _playerControl.GetNode<EPButton>(EP_BUTTON_NODE);

        _opponentDpButton = _opponentControl.GetNode<PhaseButton>(DP_BUTTON_NODE);
        _opponentSpButton = _opponentControl.GetNode<PhaseButton>(SP_BUTTON_NODE);
        _opponentM1Button = _opponentControl.GetNode<PhaseButton>(M1_BUTTON_NODE);
        _opponentBpButton = _opponentControl.GetNode<PhaseButton>(BP_BUTTON_NODE);
        _opponentM2Button = _opponentControl.GetNode<PhaseButton>(M2_BUTTON_NODE);
        _opponentEpButton = _opponentControl.GetNode<PhaseButton>(EP_BUTTON_NODE);

        //_playerInfo.Initialize(DEFAULT_PLAYER);
        //_opponentInfo.Initialize(DEFAULT_OPPONENT);

        await Delay(STARTUP_DELAY);

        await StartInitialDrawAsync();
        await DrawPhaseAsync();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
        {
            if (AttackMode)
            {
                return;
            }

            var card = GetCardAtCursor();

            if (card is not null)
            {
                if (mouseEvent.ButtonIndex is Left)
                {
                    if (card.ActionType is CardActionType.Activate)
                    {
                        if (card.CanActivate)
                        {
                            // TODO
                        }
                    }
                    else if (card.ActionType is CardActionType.Summon)
                    {
                        if (card.CanSummon)
                        {
                            SummonAndPlaceCard(card, PlayerHand, PlayerMainZone);
                        }
                    }
                    else if (card.ActionType is CardActionType.Set)
                    {
                        if (card.CanSet)
                        {
                            SummonSetAndPlaceCard(card, PlayerHand, PlayerMainZone);
                        }
                    }
                }
                else if (mouseEvent.ButtonIndex is Right)
                {
                    if (card.ActionType is CardActionType.Summon)
                    {
                        if (card.CanSet)
                        {
                            card.ActionType = CardActionType.Set;
                            card.PopupAction.ShowAction(PopupActionType.Set);
                        }
                        else if (card.CanActivate)
                        {
                            card.ActionType = CardActionType.Activate;
                            card.PopupAction.ShowAction(PopupActionType.Activate);
                        }
                    }
                    else if (card.ActionType is CardActionType.Set)
                    {
                        if (card.CanActivate)
                        {
                            card.ActionType = CardActionType.Activate;
                            card.PopupAction.ShowAction(PopupActionType.Activate);
                        }
                        else if (card.CanSummon)
                        {
                            card.ActionType = CardActionType.Summon;
                            card.PopupAction.ShowAction(PopupActionType.Summon);
                        }
                    }
                    else if (card.ActionType is CardActionType.Activate)
                    {
                        if (card.CanSummon)
                        {
                            card.ActionType = CardActionType.Summon;
                            card.PopupAction.ShowAction(PopupActionType.Summon);
                        }
                        else if (card.CanSet)
                        {
                            card.ActionType = CardActionType.Set;
                            card.PopupAction.ShowAction(PopupActionType.Set);
                        }
                    }
                }
            }
        }
    }

    public override async void _Process(double delta)
    {
        if (CurrentTurnSide is DuelSide.Opponent)
        {
            if (CurrentPhase is DuelPhase.Main1)
            {
                //Cards.Where(x=>x.DuelSide is DuelSide.Opponent&&x.Location is CardLocation.InHand)
            }

            if (CurrentPhase is DuelPhase.Battle)
            {
                await EndPhaseAsync();
            }
        }
    }
}
