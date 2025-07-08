using DMYAN.Scripts.CardInfoStack;
using DMYAN.Scripts.CardStack;
using DMYAN.Scripts.Common.Enum;
using DMYAN.Scripts.HandZoneStack;
using DMYAN.Scripts.MainDeckStack;
using DMYAN.Scripts.MainZoneStack;
using DMYAN.Scripts.ProfileStack;
using Godot;
using System.Collections.Generic;
using static DMYAN.Scripts.Common.Constant;

namespace DMYAN.Scripts.GameManagerStack;

internal partial class GameManager : DMYANNode2D
{
    [Export]
    internal MainZone PlayerMainZone { get; set; }

    [Export]
    internal MainZone OpponentMainZone { get; set; }

    [Export]
    internal CardInfo CardInfo { get; set; }

    [Export]
    private MainDeck PlayerMainDeck { get; set; }

    [Export]
    private MainDeck OpponentMainDeck { get; set; }

    [Export]
    private HandZone PlayerHand { get; set; }

    [Export]
    private HandZone OpponentHand { get; set; }

    [Export]
    private Graveyard PlayerGraveyard { get; set; }

    [Export]
    private Graveyard OpponentGraveyard { get; set; }

    [Export]
    private Profile PlayerProfile { get; set; }

    [Export]
    private Profile OpponentProfile { get; set; }

    [Export]
    private PackedScene CardScene { get; set; }

    internal DuelSide CurrentTurnSide { get; set; } = STARTING_DUEL_SIDE;

    internal DuelPhase CurrentPhase { get; set; } = DuelPhase.None;

    internal DuelStep CurrentStep { get; set; } = DuelStep.None;

    internal List<Card> Cards { get; set; } = [];

    internal Card CardAttacking { get; set; } = default;

    internal bool HasSummoned { get; set; } = false;

    internal bool IsFirstTurn { get; set; } = true;
}
