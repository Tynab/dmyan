using DMYAN.Scripts.Controls;
using DMYAN.Scripts.Popups;
using Godot;

namespace DMYAN.Scripts.GameManagerStack;

internal partial class GameManager : Node2D
{
    private Infomation _playerInfo;
    private Infomation _opponentInfo;
    private PopupPhase _popupPhase;
    private Control _control;
    private Node _playerControl;
    private Node _opponentControl;

    private DPButton _playerDPButton;
    private DPButton _playerSPButton;
    private DPButton _playerM1Button;
    private DPButton _playerBPButton;
    private DPButton _playerM2Button;
    private DPButton _playerEPButton;

    private DPButton _opponentDPButton;
    private DPButton _opponentSPButton;
    private DPButton _opponentM1Button;
    private DPButton _opponentBPButton;
    private DPButton _opponentM2Button;
    private DPButton _opponentEPButton;
}
