using DMYAN.Scripts.Common.Enum;

namespace DMYAN.Scripts.Controls;

internal partial class BPButton : PhaseButton
{
	public override void _Ready()
	{
		base._Ready();

		Pressed += OnPressed;
	}

	private async void OnPressed()
	{
		if (!IsClicked && _gameManager.CurrentPhase is not DuelPhase.Draw and not DuelPhase.Standby)
		{
			IsClicked = true;

			await _gameManager.BattlePhaseAsync();
		}
	}
}
