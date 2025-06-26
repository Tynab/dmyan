using DMYAN.Scripts.Common;
using DMYAN.Scripts.Controls;
using DMYAN.Scripts.Popups;
using Godot;
using Godot.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static DMYAN.Scripts.Common.CardDatabase;
using static DMYAN.Scripts.Common.Constant;
using static Godot.MouseButton;
using static System.Threading.Tasks.Task;

namespace DMYAN.Scripts;

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

    internal DuelSide CurrentTurnSide { get; set; } = STARTING_DUEL_SIDE;

    internal DuelPhase CurrentPhase { get; set; } = DuelPhase.None;

    internal List<Card> Cards { get; set; } = [];

    internal bool HasSummoned { get; set; } = false;

    internal bool IsFirstTurn { get; set; } = true;

    internal bool AttackMode { get; set; } = false;

    private Infomation _playerInfo;
    private Infomation _opponentInfo;
    private PopupPhase _popupPhase;
    private Control _control;

    private DpButton _dpButton;
    private SpButton _spButton;
    private M1Button _m1Button;
    private BpButton _bpButton;
    private M2Button _m2Button;
    private EpButton _epButton;

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

        _playerInfo.Initialize(DEFAULT_PLAYER);
        _opponentInfo.Initialize(DEFAULT_OPPONENT);

        LoadCards();

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

    internal async Task DrawPhaseAsync(int delay = PHASE_CHANGE_DELAY)
    {
        CurrentPhase = DuelPhase.Draw;
        HasSummoned = false;

        _dpButton.ChangeStatus(false);

        await _popupPhase.ShowPhase(PopupPhaseType.DP);

        if (CurrentTurnSide is DuelSide.Player)
        {
            await DrawAndPlaceCardAsync(PlayerDeck, PlayerHand);
        }
        else
        {
            await DrawAndPlaceCardAsync(OpponentDeck, OpponentHand);
        }

        await Delay(delay);

        await StandbyPhaseAsync();
    }

    internal async Task StandbyPhaseAsync(int delay = PHASE_CHANGE_DELAY)
    {
        CurrentPhase = DuelPhase.Standby;

        _spButton.ChangeStatus(false);

        await _popupPhase.ShowPhase(PopupPhaseType.SP);

        await Delay(delay);

        await Main1PhaseAsync();
    }

    internal async Task Main1PhaseAsync(int delay = PHASE_CHANGE_DELAY)
    {
        CurrentPhase = DuelPhase.Main1;

        _m1Button.ChangeStatus(false);

        await _popupPhase.ShowPhase(PopupPhaseType.M1);

        await Delay(delay);
    }

    internal async Task BattlePhaseAsync(int delay = PHASE_CHANGE_DELAY)
    {
        CurrentPhase = DuelPhase.Battle;

        _bpButton.ChangeStatus(false);

        await _popupPhase.ShowPhase(PopupPhaseType.BP);

        if (!IsFirstTurn)
        {
            Cards.ForEach(async x => await x.CanAttackCheck(CurrentTurnSide));
        }

        await Delay(delay);
    }

    internal async Task Main2PhaseAsync(int delay = PHASE_CHANGE_DELAY)
    {
        CurrentPhase = DuelPhase.Main2;

        Cards.Where(x => x.DuelSide == CurrentTurnSide && x.Location is CardLocation.InBoard && x.Zone is CardZone.Main && x.CardFace is CardFace.FaceUp && x.CardPosition is CardPosition.Attack)
            .ToList()
            .ForEach(async x => await x.Sword.FadeOut(OPACITY_MIN));

        _m2Button.ChangeStatus(false);

        await _popupPhase.ShowPhase(PopupPhaseType.M2);

        await Delay(delay);
    }

    internal async Task EndPhaseAsync(int delay = STARTUP_DELAY)
    {
        if (CurrentPhase is DuelPhase.Main1)
        {
            await BattlePhaseAsync(STARTUP_DELAY);
        }

        if (CurrentPhase is DuelPhase.Battle)
        {
            await Main2PhaseAsync(STARTUP_DELAY);
        }

        CurrentPhase = DuelPhase.End;

        _epButton.ChangeStatus(false);

        await _popupPhase.ShowPhase(PopupPhaseType.EP);

        await Delay(delay);

        await ChangeTurnAsync();
        await DrawPhaseAsync();
    }

    private async Task StartInitialDrawAsync()
    {
        for (var i = 0; i < INITIAL_HAND_SIZE; i++)
        {
            await DrawAndPlaceCardAsync(PlayerDeck, PlayerHand);
            await DrawAndPlaceCardAsync(OpponentDeck, OpponentHand);
        }
    }

    private static async Task DrawAndPlaceCardAsync(MainDeck deck, HandManager hand)
    {
        var card = deck.RemoveCard();

        if (card is not null)
        {
            await hand.AddCardAsync(card);
        }
    }

    private void SummonAndPlaceCard(Card card, HandManager hand, MainZone zone)
    {
        HasSummoned = true;

        hand.RemoveCard(card);
        zone.SummonCard(card);
    }

    private void SummonSetAndPlaceCard(Card card, HandManager hand, MainZone zone)
    {
        HasSummoned = true;

        hand.RemoveCard(card);
        zone.SummonSetCard(card);
    }

    private Card GetCardAtCursor()
    {
        var results = GetWorld2D().DirectSpaceState.IntersectPoint(new PhysicsPointQueryParameters2D
        {
            Position = GetGlobalMousePosition(),
            CollideWithAreas = true,
            CollisionMask = CARD_COLLISION_MASK
        });

        return results.Count > 0 ? GetTopmostCard(results) : default;
    }

    private static Card GetTopmostCard(Array<Dictionary> cards) => cards.Select(static c => c[COLLIDER_PROPERTY].As<Area2D>().GetParent<Card>()).OrderByDescending(static c => c.ZIndex).FirstOrDefault();

    private async Task ChangeTurnAsync()
    {
        _dpButton.ChangeStatus(true);
        await Delay(STARTUP_DELAY);
        _spButton.ChangeStatus(true);
        await Delay(STARTUP_DELAY);
        _m1Button.ChangeStatus(true);
        await Delay(STARTUP_DELAY);
        _bpButton.ChangeStatus(true);
        await Delay(STARTUP_DELAY);
        _m2Button.ChangeStatus(true);
        await Delay(STARTUP_DELAY);
        _epButton.ChangeStatus(true);
        await Delay(STARTUP_DELAY);
        CurrentTurnSide = CurrentTurnSide is DuelSide.Player ? DuelSide.Opponent : DuelSide.Player;
        IsFirstTurn = false;
    }
}
