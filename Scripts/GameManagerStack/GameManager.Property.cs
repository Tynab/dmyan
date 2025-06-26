using DMYAN.Scripts.Common;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DMYAN.Scripts.Common.CardDatabase;
using static DMYAN.Scripts.Common.Constant;
using static Godot.MouseButton;
using static System.Threading.Tasks.Task;

namespace DMYAN.Scripts.GameManagerStack;

internal partial class GameManager : Node2D
{
    internal DuelSide CurrentTurnSide { get; set; } = STARTING_DUEL_SIDE;

    internal DuelPhase CurrentPhase { get; set; } = DuelPhase.None;

    internal List<Card> Cards { get; set; } = [];

    internal bool HasSummoned { get; set; } = false;

    internal bool IsFirstTurn { get; set; } = true;

    internal bool AttackMode { get; set; } = false;
}
