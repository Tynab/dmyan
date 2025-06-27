using DMYAN.Scripts.Common.Enum;
using Godot;
using System.Collections.Generic;
using static DMYAN.Scripts.Common.Constant;

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
