using DMYAN.Scripts.Controls;
using DMYAN.Scripts.Popups;
using System.Collections.Generic;

namespace DMYAN.Scripts.GameManagerStack;

internal partial class GameManager : DMYANNode2D
{
    private PopupPhase _popupPhase;

    private DPButton _playerDPButton;
    private SPButton _playerSPButton;
    private M1Button _playerM1Button;
    private BPButton _playerBPButton;
    private M2Button _playerM2Button;
    private EPButton _playerEPButton;

    private PhaseButton _opponentDPButton;
    private PhaseButton _opponentSPButton;
    private PhaseButton _opponentM1Button;
    private PhaseButton _opponentBPButton;
    private PhaseButton _opponentM2Button;
    private PhaseButton _opponentEPButton;

    private List<PhaseButton> _phaseButtons;
}
