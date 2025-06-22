using Godot;
using System.Collections.Generic;
using static DMYAN.Scripts.CardDatabase;
using static DMYAN.Scripts.Constant;
using static Godot.FileAccess;
using static Godot.FileAccess.ModeFlags;
using static Godot.Vector2;

namespace DMYAN.Scripts;

public partial class MainDeck : CardSlot
{
    [Export]
    private PackedScene CardScene { get; set; }

    private readonly List<Card> _cardsInDeck = [];
    private RichTextLabel _count;

    public override void _Ready()
    {
        if (CardScene is null)
        {
            return;
        }

        LoadDeckFromCsv();
        ShuffleAndArrangeDeck();

        if (DuelSide is DuelSide.Player)
        {
            _count = GetNodeOrNull<RichTextLabel>(COUNT_LABEL_NODE);
            UpdateDeckCountDisplay();
        }
    }

    public Card DrawCard()
    {
        if (_cardsInDeck.Count is 0)
        {
            return default;
        }

        var drawnCard = _cardsInDeck[0];

        _cardsInDeck.RemoveAt(0);
        UpdateDeckCountDisplay();

        return drawnCard;
    }

    private void LoadDeckFromCsv()
    {
        _cardsInDeck.Clear();

        using var file = Open(DECKS_CSV_PATH, Read);

        if (file is null)
        {
            return;
        }

        if (!file.EofReached())
        {
            _ = file.GetLine();
        }

        while (!file.EofReached())
        {
            var line = file.GetLine();

            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            var parts = line.Split(',');
            var cardData = GetCardData(parts[1].Trim('"'));

            if (string.IsNullOrWhiteSpace(cardData.Code))
            {
                continue;
            }

            var card = CardScene.Instantiate<Card>();

            if (card is null)
            {
                continue;
            }

            card.DuelSide = DuelSide;
            card.Status = CardStatus.InDeck;
            card.Zone = CardZone.MainDeck;
            card.CardFace = CardFace.FaceDown;
            card.CardName = cardData.Name;
            card.Code = cardData.Code;
            card.Description = cardData.Description;
            card.Type = cardData.Type;
            card.Property = cardData.Property;
            card.Attribute = cardData.Attribute;
            card.Race = cardData.Race;
            card.SummonType = cardData.SummonType;
            card.Level = cardData.Level;
            card.ATK = cardData.ATK;
            card.DEF = cardData.DEF;
            card.BanlistStatus = cardData.BanlistStatus;
            card.EffectType = cardData.EffectType;

            var cardFront = card.GetNodeOrNull<Sprite2D>(CARD_FRONT_NODE);

            if (cardFront is not null)
            {
                var cardTexture = ResourceLoader.Load<Texture2D>($"res://Assets/{cardData.Code}.jpg");

                if (cardTexture is not null)
                {
                    cardFront.Texture = cardTexture;
                }

                cardFront.Visible = false;
            }

            var cardBack = card.GetNodeOrNull<Sprite2D>(CARD_BACK_NODE);

            if (cardBack is not null)
            {
                cardBack.Visible = true;
            }

            _cardsInDeck.Add(card);
            AddChild(card);
        }
    }

    private void ShuffleAndArrangeDeck()
    {
        if (_cardsInDeck.Count is 0)
        {
            return;
        }

        _cardsInDeck.Shuffle();

        for (var i = 0; i < _cardsInDeck.Count; i++)
        {
            var card = _cardsInDeck[i];

            card.Position = Zero;
            card.ZIndex = i;
        }
    }

    private void UpdateDeckCountDisplay()
    {
        if (_count is not null)
        {
            if (_cardsInDeck.Count > 0)
            {
                _count.Text = _cardsInDeck.Count.ToString();
                _count.Visible = true;
                _count.ZIndex = 1000;
            }
            else
            {
                _count.Visible = false;
            }
        }
    }
}
