using DMYAN.Scripts.Common.Enum;
using DMYAN.Scripts.Controls;
using DMYAN.Scripts.Popups;
using Godot;
using YANLib;
using static DMYAN.Scripts.Common.Constant;
using static Godot.MouseButton;
using static System.Threading.Tasks.Task;

namespace DMYAN.Scripts.GameManagerStack;

internal partial class GameManager : DMYANNode2D
{
    public override async void _Ready()
    {
        LoadData();

        var main = GetTree().Root.GetNode(MAIN_NODE);
        var button = main.GetNode<Control>(nameof(Button));
        var player = button.GetNode<Node>(DuelSide.Player.ToString());
        var opponent = button.GetNode<Node>(DuelSide.Opponent.ToString());

        _popupPhase = main.GetNode<PopupPhase>(nameof(PopupPhase));

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

        _phaseButtons =
        [
            _playerDPButton,
            _opponentDPButton,
            _playerSPButton,
            _opponentSPButton,
            _playerM1Button,
            _opponentM1Button,
            _playerBPButton,
            _opponentBPButton,
            _playerM2Button,
            _opponentM2Button,
            _playerEPButton,
            _opponentEPButton
        ];

        PlayerProfile.Init(DEFAULT_PLAYER);
        OpponentProfile.Init(DEFAULT_OPPONENT);

        SetupVisibilityButtons();

        await Delay(STARTUP_DELAY);

        await InitDraw();
        await DrawPhase();
    }

    public override async void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
        {
            var card = GetCardAtCursor();

            if (card.IsNotNull())
            {
                if (mouseEvent.ButtonIndex is Left)
                {
                    if (CardAttacking.IsNotNull() && CurrentStep is DuelStep.Attacking && card.DuelSide is DuelSide.Opponent && card.Zone is CardZone.Main)
                    {
                        await AttackStep(card);
                    }
                    else if (card.ActionType is CardActionType.Activate)
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
                            await SummonStep(card, PlayerHand, PlayerMainZone);
                        }
                    }
                    else if (card.ActionType is CardActionType.Set)
                    {
                        if (card.CanSet)
                        {
                            await SetSummonStep(card, PlayerHand, PlayerMainZone);
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
}
