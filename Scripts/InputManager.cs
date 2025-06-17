using Godot;
using System;
using static Godot.MouseButton;

namespace DMYAN.Scripts;

public partial class InputManager : Node2D
{
	[Signal]
	public delegate void LeftMouseButtonPressedEventHandler(InputManager inputManager);

	[Signal]
	public delegate void LeftMouseButtonReleasedEventHandler(InputManager inputManager);

	private const int CARD_COLLISION_MASK = 1;
	private const int DECK_COLLISION_MASK = 4;

	private CardManager _cardManagerReference;
	private Deck _deckReference;

	public override void _Ready()
	{
		_cardManagerReference = GetNode<CardManager>("../CardManager");
		_deckReference = GetNode<Deck>("../Deck");
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.ButtonIndex is Left)
		{
			if (mouseEvent.Pressed)
			{
				_ = EmitSignal(SignalName.LeftMouseButtonPressed, this);
				RaycastAtCursor();
			}
			else
			{
				_ = EmitSignal(SignalName.LeftMouseButtonReleased, this);
			}
		}
	}

	private void RaycastAtCursor()
	{
		var results = GetWorld2D().DirectSpaceState.IntersectPoint(new PhysicsPointQueryParameters2D
		{
			Position = GetGlobalMousePosition(),
			CollideWithAreas = true,
			CollisionMask = CARD_COLLISION_MASK
		});

		if (results.Count > 0)
		{
			var resultCollisionMask = results[0]["collider"].As<Area2D>().CollisionMask;

			if (resultCollisionMask is CARD_COLLISION_MASK)
			{
				var card = results[0]["collider"].As<Area2D>().GetParent<Card>();

				if (card is not null)
				{
					_cardManagerReference.StartCardDrag(card);
				}
			}
			else if(resultCollisionMask is DECK_COLLISION_MASK)
			{
				_deckReference.DrawCard();
			}
		}
	}
}
