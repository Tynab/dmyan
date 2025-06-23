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

    private Control _control;
    private Button _spButton;
    private Button _m1Button;

    public override async void _Ready()
    {
        _control = GetNodeOrNull<Control>($"../{nameof(Control)}");
        _spButton = _control.GetNodeOrNull<Button>(SP_BUTTON_NODE);
        _m1Button = _control.GetNodeOrNull<Button>(M1_BUTTON_NODE);
        LoadCards();
        await Delay(STARTUP_DELAY);
        await StartInitialDraw();
        await DrawPhase();
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

    private async Task StartInitialDraw()
    {
        for (var i = 0; i < INITIAL_HAND_SIZE; i++)
        {
            await DrawAndPlaceCard(PlayerDeck, PlayerHand);
            await DrawAndPlaceCard(OpponentDeck, OpponentHand);
        }
    }

    private static async Task DrawAndPlaceCard(MainDeck deck, HandManager hand)
    {
        var card = deck.DrawCard();

        if (card is not null)
        {
            await hand.AddCard(card);
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

    public async Task DrawPhase()
    {
        CurrentPhase = DuelPhase.Draw;
        HasSummoned = false;

        if (CurrentTurnSide is DuelSide.Player)
        {
            await DrawAndPlaceCard(PlayerDeck, PlayerHand);
        }
        else
        {
            await DrawAndPlaceCard(OpponentDeck, OpponentHand);
        }

        await Delay(PHASE_CHANGE_DELAY);
        await StandbyPhase();
    }

    public async Task StandbyPhase()
    {
        CurrentPhase = DuelPhase.Standby;
        _spButton.Disabled = true;
        await Delay(PHASE_CHANGE_DELAY);
        await Main1Phase();
    }

    public async Task Main1Phase()
    {
        CurrentPhase = DuelPhase.Main1;
        _m1Button.Disabled = true;
        await Delay(PHASE_CHANGE_DELAY);
    }

    public async Task EndPhase()
    {
        await Delay(PHASE_CHANGE_DELAY);
        CurrentTurnSide = CurrentTurnSide == DuelSide.Player ? DuelSide.Opponent : DuelSide.Player;
        await DrawPhase();
    }
}
