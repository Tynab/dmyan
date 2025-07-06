using DMYAN.Scripts.CardStack;
using DMYAN.Scripts.Common.Enum;
using DMYAN.Scripts.GameManagerStack;
using Godot;
using System;
using System.Linq;
using System.Threading.Tasks;
using YANLib;
using static DMYAN.Scripts.Common.Constant;

namespace DMYAN.Scripts;

internal partial class Graveyard : CardSlot
{
    private GameManager _gameManager;

    private RichTextLabel _count;

    public override void _Ready()
    {
        var main = GetTree().Root.GetNode(MAIN_NODE);

        _gameManager = main.GetNode<GameManager>(nameof(GameManager));

        if (DuelSide is DuelSide.Player)
        {
            _count = GetNode<RichTextLabel>(SLOT_COUNT_NODE);
        }
    }

    internal async Task AddCard(Card card, int index)
    {
        card.Reparent(this);

        await card.GraveyardEntered(index);

        UpdateCountDisplay();
    }

    private void UpdateCountDisplay()
    {
        if (_count.IsNotNull())
        {
            var cards = _gameManager.GetCardsInGraveyard(DuelSide);

            if (cards.IsNullEmpty())
            {
                _count.Hide();
            }
            else
            {
                _count.Text = cards.Count.ToString();
                _count.ZIndex = 1_000;
                _count.Show();
            }
        }
    }
}
