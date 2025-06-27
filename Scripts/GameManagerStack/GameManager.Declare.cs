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

    private DpButton _dpButton;
    private SpButton _spButton;
    private M1Button _m1Button;
    private BpButton _bpButton;
    private M2Button _m2Button;
    private EpButton _epButton;

    private Eye _playerEye;
    private Eye _opponentEye;
}
