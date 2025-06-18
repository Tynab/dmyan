using Godot;
using System.Collections.Generic;
using static DMYAN.Scripts.Constant;

namespace DMYAN.Scripts;

public partial class Deck : Node2D
{
    #region Definitions

    private readonly List<string> _cardsInDeck = ["02118022", "05901497", "07805359", "08471389"];
    private CardDatabase _cardDatabaseReference;

    #endregion

    #region Overrides

    public override void _Ready()
    {
        _cardsInDeck.Shuffle();
        GetNode<RichTextLabel>("RichTextLabel").Text = _cardsInDeck.Count.ToString();
        _cardDatabaseReference = new CardDatabase();
    }

    #endregion

    #region Public Methods

    public void DrawCard()
    {
        if (_cardsInDeck.Count is 0)
        {
            return;
        }

        var cardId = _cardsInDeck[0];

        _ = _cardsInDeck.Remove(cardId);
        GetNode<RichTextLabel>("RichTextLabel").Text = _cardsInDeck.Count.ToString();

        if (_cardsInDeck.Count is 0)
        {
            GetNode<CollisionShape2D>("Area2D/CollisionShape2D").Disabled = true;
            GetNode<Sprite2D>("Sprite2D").Visible = false;
            GetNode<RichTextLabel>("RichTextLabel").Visible = false;
        }

        var card = GD.Load<PackedScene>(CARD_SCENE_PATH).Instantiate<Card>();

        if (_cardDatabaseReference.CARDS.TryGetValue(cardId, out var stats))
        {
            card.InitializeData(cardId, stats.Atk, stats.Def);
        }
        else
        {
            card.InitializeData(cardId, 0, 0);
        }

        card.Position = Position;
        card.GetNode<AnimationPlayer>("AnimationPlayer").Play(CARD_FLIP_ANIMATION_LAYER);
        GetNode<CardManager>("../CardManager").AddChild(card);
        GetNode<PlayerHand>("../PlayerHand").AddCard(card, CARD_DRAW_ANIMATION_SPEED);
    }

    #endregion
}
