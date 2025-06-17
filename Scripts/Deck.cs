using Godot;
using System;
using System.Collections.Generic;

namespace DMYAN.Scripts;

public partial class Deck : Node2D
{
	private const string CARD_SCENE_PATH = "res://Scenes/Card.tscn";

	private readonly List<string> _cardsInDeck = ["02118022", "02118022", "02118022"];

	public override void _Ready() => GetNode<RichTextLabel>("RichTextLabel").Text = _cardsInDeck.Count.ToString();

	public void DrawCard()
	{
		_cardsInDeck.RemoveAt(0);

		if (_cardsInDeck.Count is 0)
		{
			GetNode<CollisionShape2D>("Area2D/CollisionShape2D").Disabled = true;
			GetNode<Sprite2D>("Sprite2D").Visible = false;
			GetNode<RichTextLabel>("RichTextLabel").Visible = false;
		}

		GetNode<RichTextLabel>("RichTextLabel").Text = _cardsInDeck.Count.ToString();

		var card = GD.Load<PackedScene>(CARD_SCENE_PATH).Instantiate<Card>();

		GetNode<CardManager>("../CardManager").AddChild(card);
		card.Name = "Card";
		GetNode<PlayerHand>("../PlayerHand").AddCard(card);
	}
}
