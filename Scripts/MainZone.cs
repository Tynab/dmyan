using Godot;
using System.Collections.Generic;
using System.Threading.Tasks;
using static DMYAN.Scripts.Constant;
using static Godot.GD;

namespace DMYAN.Scripts;

public partial class MainZone : Node2D
{
    [Export]
    public DuelSide DuelSide { get; set; } = DuelSide.None;

    public int CardsInZone { get; set; } = 0;

    public void CountCardsInMainZone()
    {
        for (var i = 0; i < 9; i++)
        {
            CardsInZone += GetChildOrNull<MainCardSlot>(i).CardsInSlot;
        }
    }

    public void SummonCardToMainZone(Card card)
    {
        var slot = GetChildOrNull<MainCardSlot>(RandRange(0, 4));

        while (slot.HasCardInSlot)
        {
            var rng = new RandomNumberGenerator();

            rng.Randomize();
            slot = GetChildOrNull<MainCardSlot>(rng.RandiRange(0, 4));
        }

        card.Reparent(slot);
        card.BasePosition = Vector2.Zero;
        card.Status = CardStatus.InBoard;
        card.Zone = CardZone.Main;
        card.CardFace = CardFace.FaceUp;
        card.CanSummon = false;
        card.AnimationSummon(slot.GlobalPosition, Vector2.One);
        slot.HasCardInSlot = true;
        slot.CardFaceInSlot = card.CardFace;
        slot.CardsInSlot++;
        CardsInZone++;

        //card.AnimateSummonToSlot(FindEmptyAttackSlot());
    }

    private MainCardSlot FindEmptyAttackSlot()
    {
        var emptySlots = new List<MainCardSlot>();

        for (var i = 0; i < GetChildCount(); i++)
        {
            if (GetChild(i) is MainCardSlot currentSlot && !currentSlot.HasCardInSlot)
            {
                if (i < 5 && Mathf.IsZeroApprox(currentSlot.RotationDegrees))
                {
                    emptySlots.Add(currentSlot);
                }
            }
        }

        return emptySlots.Count > 0 ? emptySlots[RandRange(0, emptySlots.Count - 1)] : default;
    }
}
