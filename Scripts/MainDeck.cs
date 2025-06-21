using Godot;
using System.Collections.Generic;
using static DMYAN.Scripts.Constant;
using static Godot.FileAccess;
using static Godot.FileAccess.ModeFlags;

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
            var cardInfo = CardDatabase.GetCardInfo(parts[1].Trim('"'));

            if (string.IsNullOrWhiteSpace(cardInfo.Code))
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
            card.Name = cardInfo.Name;
            card.Code = cardInfo.Code;
            card.Description = cardInfo.Description;
            card.Type = cardInfo.Type;
            card.Property = cardInfo.Property;
            card.Attribute = cardInfo.Attribute;
            card.Race = cardInfo.Race;
            card.SummonType = cardInfo.SummonType;
            card.Level = cardInfo.Level;
            card.ATK = cardInfo.ATK;
            card.DEF = cardInfo.DEF;
            card.BanlistStatus = cardInfo.BanlistStatus;
            card.EffectType = cardInfo.EffectType;

            var cardFront = card.GetNodeOrNull<Sprite2D>(CARD_FRONT_NODE);

            if (cardFront is not null)
            {
                var cardTexture = ResourceLoader.Load<Texture2D>($"res://Assets/{cardInfo.Code}.jpg");

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

            card.Position = Vector2.Zero;
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
                _count.ZIndex = _cardsInDeck.Count * 2;
            }
            else
            {
                _count.Visible = false;
            }
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
}
