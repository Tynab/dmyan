using DMYAN.Scripts.Common.Enum;
using DMYAN.Scripts.GameManagerStack;
using Godot;
using static DMYAN.Scripts.Common.Constant;

namespace DMYAN.Scripts.MainDeckStack;

internal partial class MainDeck : CardSlot
{
    public override void _Ready()
    {
        var main = GetTree().Root.GetNode(MAIN_NODE);

        _gameManager = main.GetNode<GameManager>(nameof(GameManager));

        if (DuelSide is DuelSide.Player)
        {
            _count = GetNode<RichTextLabel>(SLOT_COUNT_NODE);
        }
    }
}
