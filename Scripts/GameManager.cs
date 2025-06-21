using Godot;
using System.Threading.Tasks;
using static DMYAN.Scripts.Constant;
using static DMYAN.Scripts.CardDatabase;
using static System.Threading.Tasks.Task;

namespace DMYAN.Scripts;

public partial class GameManager : Node
{
    [Export]
    public MainDeck PlayerDeck { get; set; }

    [Export]
    public MainDeck OpponentDeck { get; set; }

    [Export]
    public HandManager PlayerHand { get; set; }

    [Export]
    public HandManager OpponentHand { get; set; }

    private const int INITIAL_HAND_SIZE = 5;

    public override async void _Ready()
    {
        LoadCards();

        await Delay(100);

        if (PlayerDeck is null || OpponentDeck is null || PlayerHand is null || OpponentHand is null)
        {
            return;
        }

        await StartInitialDraw();
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
            await hand.AddCardToHand(card);
        }
    }
}
