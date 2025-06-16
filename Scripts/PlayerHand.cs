using Godot;
using System.Collections.Generic;

namespace DMYAN.Scripts;

public partial class PlayerHand : Node2D
{
	private const int HAND_SIZE = 5;
	private const int CARD_W = 80;
	private const int HAND_Y = 820;
	private const string CARD_SCENE_PATH = "res://Scenes/Card.tscn";

	private readonly List<Card> _cardsInHand = new(HAND_SIZE);
	private float _screenCenterX;

	public override void _Ready()
	{
		_screenCenterX = GetViewportRect().Size.X / 2;

		var cardScene = GD.Load<PackedScene>(CARD_SCENE_PATH);

		for (var i = 0; i < HAND_SIZE; i++)
		{
			var card = cardScene.Instantiate<Card>();

			GetNode<CardManager>("../CardManager").AddChild(card);
			card.Name = "Card";
			AddCard(card);
		}
	}

	public void AddCard(Card card)
	{
		if (!_cardsInHand.Contains(card))
		{
			_cardsInHand.Insert(0, card);
			UpdateCardPositions();
		}
		else
		{
			AnimateCardPositions(card, card.StartingPosition);
		}
	}

	public void RemoveCard(Card card)
	{
		if (_cardsInHand.Remove(card))
		{
			UpdateCardPositions();
		}
	}

	private void UpdateCardPositions()
	{
		for (var i = 0; i < _cardsInHand.Count; i++)
		{
			var card = _cardsInHand[i];
			var position = new Vector2(CalculateCardPosition(i), HAND_Y);

			card.StartingPosition = position;
			AnimateCardPositions(card, position);
		}
	}

	private float CalculateCardPosition(int index) => _screenCenterX + index * CARD_W - (_cardsInHand.Count - 1) * CARD_W / 2;

	private void AnimateCardPositions(Card card, Vector2 position) => GetTree().CreateTween().TweenProperty(card, "position", position, 0.2f);
}
