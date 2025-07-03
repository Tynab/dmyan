using DMYAN.Scripts.CardStack;
using DMYAN.Scripts.Common.Enum;
using DMYAN.Scripts.Controls;
using Godot;
using System.Collections.Generic;
using static DMYAN.Scripts.Common.Constant;

namespace DMYAN.Scripts.GameManagerStack;

internal partial class GameManager : Node2D
{
    [Export]
    private MainDeck PlayerMainDeck { get; set; }

    [Export]
    private MainDeck OpponentMainDeck { get; set; }

    [Export]
    private HandManager PlayerHand { get; set; }

    [Export]
    private HandManager OpponentHand { get; set; }

    [Export]
    private MainZone PlayerMainZone { get; set; }

    [Export]
    private MainZone OpponentMainZone { get; set; }

    private DPButton _playerDPButton;

    private DPButton _opponentDPButton;

    private DPButton _playerSPButton;

    private DPButton _opponentSPButton;

    private DPButton _playerM1Button;

    private DPButton _opponentM1Button;

    private DPButton _playerBPButton;

    private DPButton _opponentBPButton;

    private DPButton _playerM2Button;

    private DPButton _opponentM2Button;

    private DPButton _playerEPButton;

    private DPButton _opponentEPButton;

    [Export]
    private Infomation PlayerInfo { get; set; }

    [Export]
    private Infomation OpponentInfo { get; set; }

    [Export]
    private CardInfo CardInfo { get; set; }

    [Export]
    private PackedScene CardScene { get; set; }

    internal DuelSide CurrentTurnSide { get; set; } = STARTING_DUEL_SIDE;

    internal DuelPhase CurrentPhase { get; set; } = DuelPhase.None;

    internal List<Card> Cards { get; set; } = [];

    internal bool HasSummoned { get; set; } = false;

    internal bool IsFirstTurn { get; set; } = true;

    internal bool AttackMode { get; set; } = false;
}
