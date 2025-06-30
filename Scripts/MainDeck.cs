using DMYAN.Scripts.CardStack;
using DMYAN.Scripts.Common;
using DMYAN.Scripts.Common.Enum;
using Godot;
using System.Collections.Generic;
using static DMYAN.Scripts.Common.CardDatabase;
using static DMYAN.Scripts.Common.Constant;
using static Godot.FileAccess;
using static Godot.FileAccess.ModeFlags;
using static Godot.ResourceLoader;
using static Godot.Vector2;

namespace DMYAN.Scripts;

internal partial class MainDeck : CardSlot
{
    [Export]
    private PackedScene CardScene { get; set; }

    internal List<Card> CardsInDeck { get; set; } = [];

    private RichTextLabel _count;

    public override void _Ready()
    {
        LoadData();
        ShuffleAndArrange();

        if (DuelSide is DuelSide.Player)
        {
            _count = GetNode<RichTextLabel>(SLOT_COUNT_NODE);

            UpdateCountDisplay();
        }
    }

    internal Card RemoveCard()
    {
        if (CardsInDeck.Count is 0)
        {
            return default;
        }

        var drawnCard = CardsInDeck[0];

        CardsInDeck.RemoveAt(0);

        UpdateCountDisplay();

        return drawnCard;
    }

    private void LoadData()
    {
        CardsInDeck.Clear();

        using var file = Open(DECKS_DATA_PATH, Read);

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
            var card = CardScene.Instantiate<Card>();

            card.DuelSide = DuelSide;
            card.Location = CardLocation.InDeck;
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
            card.BaseATK = cardData.ATK;
            card.BaseDEF = cardData.DEF;
            card.BanlistStatus = cardData.BanlistStatus;
            card.EffectType = cardData.EffectType;

            var cardFront = card.GetNode<Sprite2D>(CARD_FRONT_NODE);

            cardFront.Texture = Load<Texture2D>(cardData.Code.GetCardAssetPathByCode());
            cardFront.Hide();

            var cardBack = card.GetNode<Sprite2D>(CARD_BACK_NODE);

            cardBack.Show();

            CardsInDeck.Add(card);

            AddChild(card);
        }
    }

    private void ShuffleAndArrange()
    {
        if (CardsInDeck.Count is 0)
        {
            return;
        }

        CardsInDeck.Shuffle();

        for (var i = 0; i < CardsInDeck.Count; i++)
        {
            var card = CardsInDeck[i];

            card.Position = Zero;
            card.ZIndex = i;
        }
    }

    private void UpdateCountDisplay()
    {
        if (_count is not null)
        {
            if (CardsInDeck.Count > 0)
            {
                _count.Text = CardsInDeck.Count.ToString();
                _count.ZIndex = 1000;
                _count.Show();
            }
            else
            {
                _count.Hide();
            }
        }
    }
}
