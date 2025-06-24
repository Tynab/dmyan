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
    private Button _dpButton;
    private Button _spButton;
    private Button _m1Button;
    private Button _bpButton;
    private Button _m2Button;
    private Button _epButton;

    public override async void _Ready()
    {
        _control = GetNodeOrNull<Control>($"../{nameof(Control)}");
        _dpButton = _control.GetNodeOrNull<Button>(DP_BUTTON_NODE);
        _spButton = _control.GetNodeOrNull<Button>(SP_BUTTON_NODE);
        _m1Button = _control.GetNodeOrNull<Button>(M1_BUTTON_NODE);
        _bpButton = _control.GetNodeOrNull<Button>(BP_BUTTON_NODE);
        _m2Button = _control.GetNodeOrNull<Button>(M2_BUTTON_NODE);
        _epButton = _control.GetNodeOrNull<Button>(EP_BUTTON_NODE);
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
        HasSummoned = false;

        if (CurrentTurnSide is DuelSide.Player)
        {
            await DrawAndPlaceCardAsync(PlayerDeck, PlayerHand);
        }
        else
        {
            await DrawAndPlaceCardAsync(OpponentDeck, OpponentHand);
        }

        ButtonDisable(_dpButton);
        await Delay(delay);
        await StandbyPhaseAsync();
    }

    public async Task StandbyPhaseAsync(int delay = PHASE_CHANGE_DELAY)
    {
        CurrentPhase = DuelPhase.Standby;
        ButtonDisable(_spButton);
        await Delay(delay);
        await Main1PhaseAsync();
    }

    public async Task Main1PhaseAsync(int delay = PHASE_CHANGE_DELAY)
    {
        CurrentPhase = DuelPhase.Main1;
        ButtonDisable(_m1Button);
        await Delay(delay);
    }

    public async Task BattlePhaseAsync(int delay = PHASE_CHANGE_DELAY)
    {
        CurrentPhase = DuelPhase.Battle;
        ButtonDisable(_bpButton);
        await Delay(delay);
    }

    public async Task Main2PhaseAsync(int delay = PHASE_CHANGE_DELAY)
    {
        CurrentPhase = DuelPhase.Main2;
        ButtonDisable(_m2Button);
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
        ButtonDisable(_epButton);
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
        ButtonEnable(_dpButton);
        await Delay(STARTUP_DELAY);
        ButtonEnable(_spButton);
        await Delay(STARTUP_DELAY);
        ButtonEnable(_m1Button);
        await Delay(STARTUP_DELAY);
        ButtonEnable(_bpButton);
        await Delay(STARTUP_DELAY);
        ButtonEnable(_m2Button);
        await Delay(STARTUP_DELAY);
        ButtonEnable(_epButton);
        await Delay(STARTUP_DELAY);
        CurrentTurnSide = CurrentTurnSide is DuelSide.Player ? DuelSide.Opponent : DuelSide.Player;
        IsFirstTurn = false;
    }

    private static void ButtonEnable(Button button)
    {
        button.Disabled = false;
        button.AddThemeFontSizeOverride("font_size", 40);
    }

    private static void ButtonDisable(Button button)
    {
        button.Disabled = true;
        button.AddThemeFontSizeOverride("font_size", 35);
    }
}
