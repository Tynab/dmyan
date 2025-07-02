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
}
