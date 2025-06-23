using Godot;
using System.Collections.Generic;
using static Godot.GD;
using static Godot.Vector2;

namespace DMYAN.Scripts;

public partial class MainZone : Node2D
{
    [Export]
    public DuelSide DuelSide { get; set; } = DuelSide.None;

    public int CardsInZone { get; set; } = 0;

    private Node2D _powerZone;

    public override void _Ready() => _powerZone = GetNodeOrNull<Node2D>("../PowerZone");

    public void SummonCard(Card card)
    {
        var slot = FindEmptyAttackSlot();

        card.Reparent(slot);
        card.BasePosition = Zero;
        card.Status = CardStatus.InBoard;
        card.Zone = CardZone.Main;
        card.CardFace = CardFace.FaceUp;
        card.CanSummon = false;
        card.AnimationSummon(slot.GlobalPosition, One);
        slot.HasCardInSlot = true;
        slot.CardFaceInSlot = card.CardFace;
        slot.CardsInSlot++;
        CardsInZone++;
        _powerZone.GetChildOrNull<PowerSlot>(slot.Index).ShowAtk(card);
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
