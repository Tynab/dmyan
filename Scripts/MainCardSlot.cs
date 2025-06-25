using Godot;

namespace DMYAN.Scripts;

public partial class MainCardSlot : CardSlot
{
	[Export]
	public PowerSlot PowerSlot { get; set; }

	public int CardsInSlot { get; set; } = 0;

	public bool HasCardCanAttack { get; set; } = false;

	public void SummonCard(Card card)
	{
		card.Summon(this);
		HasCardInSlot = true;
		CardsInSlot++;
		PowerSlot.ShowPower(card, true);
	}

	public void SummonSetCard(Card card)
	{
		card.SummonSet(this);
		HasCardInSlot = true;
		CardsInSlot++;

		if (DuelSide is DuelSide.Player)
		{
			PowerSlot.ShowPower(card, false);
		}
	}
}
