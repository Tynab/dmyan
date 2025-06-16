using Godot;
using Godot.Collections;
using System.Linq;
using static Godot.MouseButton;
using static System.Math;

namespace DMYAN.Scripts;

public partial class CardManager : Node2D
{
	private const int CARD_COLLISION_MASK = 1;
	private const int CARD_SLOT_V_COLLISION_MASK = 2;

	private Vector2 _screenSize;
	private Card _selectedCard;
	private bool _isHoveringCard;
	private PlayerHand _playerHandReference;

	public override void _Ready()
	{
		_screenSize = GetViewportRect().Size;
		_playerHandReference = GetNode<PlayerHand>("../PlayerHand");
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.ButtonIndex is Left)
		{
			if (mouseEvent.Pressed)
			{
				var card = GetCardAtCursor();

				if (card is not null)
				{
					StartCardDrag(card);
				}
			}
			else if (_selectedCard is not null)
			{
				StopCardDrag();
			}
		}
	}

	public override void _Process(double delta)
	{
		if (_selectedCard is not null)
		{
			var mousePosition = GetGlobalMousePosition();

			_selectedCard.Position = new Vector2(Clamp(mousePosition.X, 0, _screenSize.X), Clamp(mousePosition.Y, 0, _screenSize.Y));
		}
	}

	public void ConnectCardSignals(Card card)
	{
		card.Hovered += OnCardHovered;
		card.Unhovered += OnCardUnhovered;
	}

	private void OnCardHovered(Card card)
	{
		if (!_isHoveringCard)
		{
			_isHoveringCard = true;
			HighlightCard(card, true);
		}
	}

	private void OnCardUnhovered(Card card)
	{
		if (_selectedCard is null)
		{
			HighlightCard(card, false);

			var cardHovered = GetCardAtCursor();

			if (cardHovered is not null)
			{
				HighlightCard(cardHovered, true);
			}
			else
			{
				_isHoveringCard = false;
			}
		}
	}

	private static void HighlightCard(Card card, bool hovered)
	{
		if (hovered)
		{
			card.Scale = new Vector2(1.05f, 1.05f);
			card.ZIndex = 2;
		}
		else
		{
			card.Scale = Vector2.One;
			card.ZIndex = 1;
		}
	}

	private CardSlotV GetCardSlotVAtCursor()
	{
		var results = GetWorld2D().DirectSpaceState.IntersectPoint(new PhysicsPointQueryParameters2D
		{
			Position = GetGlobalMousePosition(),
			CollideWithAreas = true,
			CollisionMask = CARD_SLOT_V_COLLISION_MASK
		});

		return results.Count > 0 ? results.Select(static c => c["collider"].As<Area2D>().GetParent<CardSlotV>()).FirstOrDefault() : null;
	}

	private Card GetCardAtCursor()
	{
		var results = GetWorld2D().DirectSpaceState.IntersectPoint(new PhysicsPointQueryParameters2D
		{
			Position = GetGlobalMousePosition(),
			CollideWithAreas = true,
			CollisionMask = CARD_COLLISION_MASK
		});

		return results.Count > 0 ? GetTopmostCard(results) : null;
	}

	private static Card GetTopmostCard(Array<Dictionary> cards) => cards.Select(static c => c["collider"].As<Area2D>().GetParent<Card>()).OrderByDescending(static c => c.ZIndex).FirstOrDefault();

	private void StartCardDrag(Card card)
	{
		_selectedCard = card;
		card.Scale = Vector2.One;
	}

	private void StopCardDrag()
	{
		_selectedCard.Scale = new Vector2(1.05f, 1.05f);

		var cardSlotV = GetCardSlotVAtCursor();

		if (cardSlotV is not null && !cardSlotV.CardInSlot)
		{
			_playerHandReference.RemoveCard(_selectedCard);
			_selectedCard.Position = cardSlotV.GlobalPosition;
			_selectedCard.GetNode<CollisionShape2D>("Area2D/CollisionShape2D").Disabled = true;
			cardSlotV.CardInSlot = true;
		}
		else
		{
			_playerHandReference.AddCard(_selectedCard);
		}

		_selectedCard = null;
	}
}
