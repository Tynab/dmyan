using Godot;
using Godot.Collections;
using System.Linq;
using static DMYAN.Scripts.Constant;
using static System.Math;

namespace DMYAN.Scripts;

public partial class CardManager : Node2D
{
    #region Definitions

    private Vector2 _screenSize;
    private Card _selectedCard;
    private bool _isHoveringCard;
    private PlayerHand _playerHandReference;
    private bool _playedMonsterCardThisTurn = false;

    #endregion

    #region Signal Handlers

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
        if (card.CardSlotCardIsIn is null && _selectedCard is null)
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

    private void OnLeftMouseButtonReleased(InputManager inputManager)
    {
        if (_selectedCard is not null)
        {
            StopCardDrag();
        }
    }

    #endregion

    #region Overrides

    public override void _Ready()
    {
        _screenSize = GetViewportRect().Size;
        _playerHandReference = GetNode<PlayerHand>("../PlayerHand");
        GetNode<InputManager>("../InputManager").LeftMouseButtonReleased += OnLeftMouseButtonReleased;

    }

    public override void _Process(double delta)
    {
        if (_selectedCard is not null)
        {
            var mousePosition = GetGlobalMousePosition();

            _selectedCard.Position = new Vector2(Clamp(mousePosition.X, 0, _screenSize.X), Clamp(mousePosition.Y, 0, _screenSize.Y));
        }
    }

    #endregion

    #region Public Methods

    public Card GetCardAtCursor()
    {
        var results = GetWorld2D().DirectSpaceState.IntersectPoint(new PhysicsPointQueryParameters2D
        {
            Position = GetGlobalMousePosition(),
            CollideWithAreas = true,
            CollisionMask = CARD_COLLISION_MASK
        });

        return results.Count > 0 ? GetTopmostCard(results) : null;
    }

    public void StartCardDrag(Card card)
    {
        _selectedCard = card;
        card.Scale = Vector2.One;
        card.ZIndex = 100;
    }

    public void StopCardDrag()
    {
        if (_selectedCard is null)
        {
            return;
        }

        _selectedCard.Scale = new Vector2(CARD_HOVERED_SCALE, CARD_HOVERED_SCALE);

        var cardSlotV = GetCardSlotVAtCursor();
        //var cardSlotH = GetCardSlotHAtCursor();

        if (cardSlotV is not null && !cardSlotV.CardInSlot)
        {
            if (_selectedCard.Type == cardSlotV.Type)
            {
                _selectedCard.CardSlotCardIsIn = cardSlotV;
                PlaceCardInSlot(_selectedCard, cardSlotV, cardSlotV.GlobalPosition, true);
                _selectedCard = null;

                return;
            }
            else
            {
                _playerHandReference.AddCard(_selectedCard, CARD_DEFAULT_ANIMATION_SPEED);
            }
        }
        //else if (cardSlotH is not null && !cardSlotH.CardInSlot)
        //{
        //    PlaceCardInSlot(_selectedCard, cardSlotH, cardSlotH.GlobalPosition, false);
        //}
        _playerHandReference.AddCard(_selectedCard, CARD_DEFAULT_ANIMATION_SPEED);
        _selectedCard = null;
    }

    #endregion

    #region Private Methods

    private void PlaceCardInSlot(Card card, Node2D slot, Vector2 position, bool isAttackPosition)
    {
        _playerHandReference.RemoveCard(card);
        card.Position = position;
        card.GetNode<CollisionShape2D>("Area2D/CollisionShape2D").Disabled = true;
        card.ZIndex = 0;

        if (slot is MainCardSlotV slotV)
        {
            slotV.CardInSlot = true;
        }
        else if (slot is CardSlotH slotH)
        {
            slotH.CardInSlot = true;
        }

        card.SetStatsVisibility(true);
        card.SetStatsColorForPosition(isAttackPosition);
    }

    private static void HighlightCard(Card card, bool hovered)
    {
        if (hovered)
        {
            card.Scale = new Vector2(CARD_HOVERED_SCALE, CARD_HOVERED_SCALE);
            card.ZIndex = 2;
        }
        else
        {
            card.Scale = Vector2.One;
            card.ZIndex = 1;
        }
    }

    private MainCardSlotV GetCardSlotVAtCursor()
    {
        var result = GetWorld2D().DirectSpaceState.IntersectPoint(new PhysicsPointQueryParameters2D
        {
            Position = GetGlobalMousePosition(),
            CollideWithAreas = true,
            CollisionMask = CARD_SLOT_V_COLLISION_MASK
        });

        return result.Count > 0 ? result[0]["collider"].As<Area2D>().GetParent<MainCardSlotV>() : null;
    }

    private CardSlotH GetCardSlotHAtCursor()
    {
        var result = GetWorld2D().DirectSpaceState.IntersectPoint(new PhysicsPointQueryParameters2D
        {
            Position = GetGlobalMousePosition(),
            CollideWithAreas = true,
            CollisionMask = CARD_SLOT_H_COLLISION_MASK
        });
        return result.Count > 0 ? result[0]["collider"].As<Area2D>().GetParent<CardSlotH>() : null;
    }

    private static Card GetTopmostCard(Array<Dictionary> cards) => cards.Select(static c => c["collider"].As<Area2D>().GetParent<Card>()).OrderByDescending(static c => c.ZIndex).FirstOrDefault();

    #endregion
}
