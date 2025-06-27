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
    [Export]
    private MainDeck PlayerDeck { get; set; }

    [Export]
    private MainDeck OpponentDeck { get; set; }

    [Export]
    private HandManager PlayerHand { get; set; }

    [Export]
    private HandManager OpponentHand { get; set; }

    [Export]
    private MainZone PlayerMainZone { get; set; }

    [Export]
    private MainZone OpponentMainZone { get; set; }

    public override async void _Ready()
    {
        var parent = GetParent();
        var board = parent.GetNode<Node2D>(BOARD_NODE);
        var playerField = board.GetNode<Node2D>(FIELD_PLAYER_NODE);
        var opponentField = board.GetNode<Node2D>(FIELD_OPPONENT_NODE);

        Cards.AddRange(playerField.GetNode<MainDeck>(nameof(MainDeck)).CardsInDeck);
        Cards.AddRange(opponentField.GetNode<MainDeck>(nameof(MainDeck)).CardsInDeck);

        _playerInfo = parent.GetNode<Infomation>(INFO_PLAYER_NODE);
        _opponentInfo = parent.GetNode<Infomation>(INFO_OPPONENT_NODE);
        _popupPhase = parent.GetNode<PopupPhase>(nameof(PopupPhase));
        _control = parent.GetNode<Control>(nameof(Control));

        _dpButton = _control.GetNode<DpButton>(DP_BUTTON_NODE);
        _spButton = _control.GetNode<SpButton>(SP_BUTTON_NODE);
        _m1Button = _control.GetNode<M1Button>(M1_BUTTON_NODE);
        _bpButton = _control.GetNode<BpButton>(BP_BUTTON_NODE);
        _m2Button = _control.GetNode<M2Button>(M2_BUTTON_NODE);
        _epButton = _control.GetNode<EpButton>(EP_BUTTON_NODE);

        _playerEye = playerField.GetNode<Eye>(nameof(Eye));
        _opponentEye = opponentField.GetNode<Eye>(nameof(Eye));

        _playerInfo.Initialize(DEFAULT_PLAYER);
        _opponentInfo.Initialize(DEFAULT_OPPONENT);

        LoadCards();

        await Delay(STARTUP_DELAY);

        await StartInitialDrawAsync();

        HighlightEye();

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
}
