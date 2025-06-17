using Godot;
using System.Collections.Generic;
using static Godot.Tween.EaseType;
using static Godot.Tween.TransitionType;

namespace DMYAN.Scripts;

public partial class PlayerHand : Node2D
{
	private const int CARD_W = 80;
	private const int HAND_Y = 820;
	private const double DEFAULT_CARD_ANIMATION_SPEED = 0.1;

	private readonly List<Card> _cardsInHand = [];
	private float _screenCenterX;

	public override void _Ready() => _screenCenterX = GetViewportRect().Size.X / 2;

	public void AddCard(Card card, double speed)
	{
		if (!_cardsInHand.Contains(card))
		{
			_cardsInHand.Insert(0, card);
		}

		UpdateCardPositions(speed);
	}

	public void RemoveCard(Card card)
	{
		if (_cardsInHand.Remove(card))
		{
			UpdateCardPositions(DEFAULT_CARD_ANIMATION_SPEED);
		}
	}

	private void UpdateCardPositions(double speed)
	{
		for (var i = 0; i < _cardsInHand.Count; i++)
		{
			var card = _cardsInHand[i];
			var position = new Vector2(CalculateCardPosition(i), HAND_Y);

			card.StartingPosition = position;
			AnimateCardPositions(card, position, speed);
		}
	}

	private float CalculateCardPosition(int index) => _screenCenterX + index * CARD_W - (_cardsInHand.Count - 1) * CARD_W / 2;

	private void AnimateCardPositions(Card card, Vector2 position, double speed) => GetTree().CreateTween().SetTrans(Circ).SetEase(Out).TweenProperty(card, "position", position, speed);
}
