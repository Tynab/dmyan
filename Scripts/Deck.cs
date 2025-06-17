using Godot;
using System.Collections.Generic;

namespace DMYAN.Scripts;

public partial class Deck : Node2D
{
	private const string CARD_SCENE_PATH = "res://Scenes/Card.tscn";
	private const double CARD_DRAW_SPEED = 0.2;

	private readonly List<string> _cardsInDeck = ["02118022", "05901497", "07805359", "08471389"];
	private Resource _cardDatabaseReference;

	public override void _Ready()
	{
		_cardsInDeck.Shuffle();
		GetNode<RichTextLabel>("RichTextLabel").Text = _cardsInDeck.Count.ToString();
		_cardDatabaseReference = GD.Load("res://Scenes/CardDatabase.cs");
	}

	public void DrawCard()
	{
		if (_cardsInDeck.Count is 0)
		{
			return;
		}

		var cardDrawnName = _cardsInDeck[0];

		_ = _cardsInDeck.Remove(cardDrawnName);
		GetNode<RichTextLabel>("RichTextLabel").Text = _cardsInDeck.Count.ToString();

		if (_cardsInDeck.Count is 0)
		{
			GetNode<CollisionShape2D>("Area2D/CollisionShape2D").Disabled = true;
			GetNode<Sprite2D>("Sprite2D").Visible = false;
			GetNode<RichTextLabel>("RichTextLabel").Visible = false;
		}

		var card = GD.Load<PackedScene>(CARD_SCENE_PATH).Instantiate<Card>();

		card.Position = Position;
		card.GetNode<Sprite2D>("Sprite2D").Texture = GD.Load<Texture2D>($"res://Assets/{cardDrawnName}.jpg");
		GetNode<CardManager>("../CardManager").AddChild(card);
		GetNode<PlayerHand>("../PlayerHand").AddCard(card, CARD_DRAW_SPEED);
		card.GetNode<AnimationPlayer>("AnimationPlayer").Play("card_flip");
	}
}
