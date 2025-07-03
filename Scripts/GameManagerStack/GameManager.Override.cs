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

        var main = GetTree().Root.GetNode(MAIN_NODE);
        var control = main.GetNode<Control>(nameof(Control));
        var button = control.GetNode<Control>(nameof(Button));
        var player = button.GetNode<Node>(DuelSide.Player.ToString());
        var opponent = button.GetNode<Node>(DuelSide.Opponent.ToString());

        _playerDPButton = player.GetNode<DPButton>(nameof(DPButton));
        _playerSPButton = player.GetNode<SPButton>(nameof(SPButton));
        _playerM1Button = player.GetNode<M1Button>(nameof(M1Button));
        _playerBPButton = player.GetNode<BPButton>(nameof(BPButton));
        _playerM2Button = player.GetNode<M2Button>(nameof(M2Button));
        _playerEPButton = player.GetNode<EPButton>(nameof(EPButton));

        _opponentDPButton = opponent.GetNode<PhaseButton>(nameof(DPButton));
        _opponentSPButton = opponent.GetNode<PhaseButton>(nameof(SPButton));
        _opponentM1Button = opponent.GetNode<PhaseButton>(nameof(M1Button));
        _opponentBPButton = opponent.GetNode<PhaseButton>(nameof(BPButton));
        _opponentM2Button = opponent.GetNode<PhaseButton>(nameof(M2Button));
        _opponentEPButton = opponent.GetNode<PhaseButton>(nameof(EPButton));

        _popupPhase = main.GetNode<PopupPhase>(nameof(PopupPhase));

        PlayerInfo.Init(DEFAULT_PLAYER);
        OpponentInfo.Init(DEFAULT_OPPONENT);

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
