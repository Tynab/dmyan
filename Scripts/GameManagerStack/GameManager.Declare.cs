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

    private DpButton _playerDpButton;
    private SpButton _playerSpButton;
    private M1Button _playerM1Button;
    private BpButton _playerBpButton;
    private M2Button _playerM2Button;
    private EpButton _playerEpButton;

    private PhaseButton _opponentDpButton;
    private PhaseButton _opponentSpButton;
    private PhaseButton _opponentM1Button;
    private PhaseButton _opponentBpButton;
    private PhaseButton _opponentM2Button;
    private PhaseButton _opponentEpButton;
}
