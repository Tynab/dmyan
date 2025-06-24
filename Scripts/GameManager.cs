using DMYAN.Scripts.Controls;
using DMYAN.Scripts.Popups;
using Godot;
using Godot.Collections;
using System.Linq;
using System.Threading.Tasks;
using static DMYAN.Scripts.CardDatabase;
using static DMYAN.Scripts.Constant;
using static Godot.MouseButton;
using static System.Threading.Tasks.Task;

namespace DMYAN.Scripts;

public partial class GameManager : Node2D
{
    [Export]
    public MainDeck PlayerDeck { get; set; }

    [Export]
    public MainDeck OpponentDeck { get; set; }

    [Export]
    public HandManager PlayerHand { get; set; }

    [Export]
    public HandManager OpponentHand { get; set; }

    [Export]
    public MainZone PlayerMainZone { get; set; }

    [Export]
    public MainZone OpponentMainZone { get; set; }

    public DuelSide CurrentTurnSide { get; set; } = STARTING_DUEL_SIDE;

    public DuelPhase CurrentPhase { get; set; } = DuelPhase.None;

    public bool HasSummoned { get; set; } = false;

    public bool IsFirstTurn { get; set; } = true;

    private Control _control;
    private DpButton _dpButton;
    private SpButton _spButton;
    private M1Button _m1Button;
    private BpButton _bpButton;
    private M2Button _m2Button;
    private EpButton _epButton;
    private PopupPhase _popupPhase;

    public override async void _Ready()
    {
        _control = GetNode<Control>($"../{nameof(Control)}");
        _dpButton = _control.GetNode<DpButton>(DP_BUTTON_NODE);
        _spButton = _control.GetNode<SpButton>(SP_BUTTON_NODE);
        _m1Button = _control.GetNode<M1Button>(M1_BUTTON_NODE);
        _bpButton = _control.GetNode<BpButton>(BP_BUTTON_NODE);
        _m2Button = _control.GetNode<M2Button>(M2_BUTTON_NODE);
        _epButton = _control.GetNode<EpButton>(EP_BUTTON_NODE);
        _popupPhase = _control.GetNode<PopupPhase>($"../{nameof(PopupPhase)}");
        LoadCards();
        await Delay(STARTUP_DELAY);
        await StartInitialDrawAsync();
        await DrawPhaseAsync();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed && mouseEvent.ButtonIndex is Left)
        {
            var card = GetCardAtCursor();

            if (card is not null && card.CanSummon)
            {
                SummonAndPlaceCard(card, PlayerHand, PlayerMainZone);
            }
        }
    }

    public async Task DrawPhaseAsync(int delay = PHASE_CHANGE_DELAY)
    {
        CurrentPhase = DuelPhase.Draw;
        _dpButton.Disable();
        await _popupPhase.ShowDPPopup();
        HasSummoned = false;

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

    public async Task StandbyPhaseAsync(int delay = PHASE_CHANGE_DELAY)
    {
        CurrentPhase = DuelPhase.Standby;
        _spButton.Disable();
        await Delay(delay);
        await Main1PhaseAsync();
    }

    public async Task Main1PhaseAsync(int delay = PHASE_CHANGE_DELAY)
    {
        CurrentPhase = DuelPhase.Main1;
        _m1Button.Disable();
        await Delay(delay);
    }

    public async Task BattlePhaseAsync(int delay = PHASE_CHANGE_DELAY)
    {
        CurrentPhase = DuelPhase.Battle;
        _bpButton.Disable();
        await Delay(delay);
    }

    public async Task Main2PhaseAsync(int delay = PHASE_CHANGE_DELAY)
    {
        CurrentPhase = DuelPhase.Main2;
        _m2Button.Disable();
        await Delay(delay);
    }

    public async Task EndPhaseAsync(int delay = STARTUP_DELAY)
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
        _epButton.Disable();
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
        var card = deck.DrawCard();

        if (card is not null)
        {
            await hand.AddCardAsync(card);
        }
    }

    private void SummonAndPlaceCard(Card card, HandManager hand, MainZone zone)
    {
        hand.RemoveCard(card);
        zone.SummonCard(card);
        HasSummoned = true;
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

    private static Card GetTopmostCard(Array<Dictionary> cards) => cards.Select(static c => c["collider"].As<Area2D>().GetParent<Card>()).OrderByDescending(static c => c.ZIndex).FirstOrDefault();

    private async Task ChangeTurnAsync()
    {
        _dpButton.Disable();
        await Delay(STARTUP_DELAY);
        _spButton.Disable();
        await Delay(STARTUP_DELAY);
        _m1Button.Disable();
        await Delay(STARTUP_DELAY);
        _bpButton.Disable();
        await Delay(STARTUP_DELAY);
        _m2Button.Disable();
        await Delay(STARTUP_DELAY);
        _epButton.Disable();
        await Delay(STARTUP_DELAY);
        CurrentTurnSide = CurrentTurnSide is DuelSide.Player ? DuelSide.Opponent : DuelSide.Player;
        IsFirstTurn = false;
    }
}
