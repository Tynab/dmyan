using Godot;
using System;
using static DMYAN.Scripts.Constant;
using static Godot.Colors;

namespace DMYAN.Scripts;

public partial class Card : Node2D
{
	#region Definitions

	private RichTextLabel _atkLabel;
	private RichTextLabel _defLabel;
	private RichTextLabel _slashLabel;

	#endregion

	#region Event Handlers

	[Signal]
	public delegate void HoveredEventHandler(Card card);

	[Signal]
	public delegate void UnhoveredEventHandler(Card card);

	#endregion

	#region Signal Handlers

	private void OnArea2DMouseEntered() => EmitSignal(SignalName.Hovered, this);

	private void OnArea2DMouseExited() => EmitSignal(SignalName.Unhovered, this);

	#endregion

	#region Properties

	public string CardId { get; private set; }

	public int Atk { get; private set; }

	public int Def { get; private set; }

	public Vector2 StartingPosition { get; set; }

	#endregion

	#region Overrides

	public override void _Ready()
	{
		_ = GetParent().Call("ConnectCardSignals", this);
		_atkLabel = GetNode<RichTextLabel>("Atk");
		_defLabel = GetNode<RichTextLabel>("Def");
		_slashLabel = GetNode<RichTextLabel>("_");
		SetStatsVisibility(false);
	}

	#endregion

	#region Public Methods

	public void InitializeData(string cardId, int atk, int def)
	{
		CardId = cardId;
		Atk = atk;
		Def = def;

		if (_atkLabel is not null)
		{
			_atkLabel.Text = atk.ToString();
		}

		if (_defLabel is not null)
		{
			_defLabel.Text = def.ToString();
		}

		var sprite = GetNode<Sprite2D>("CardImage");

		try
		{
			sprite.Texture = GD.Load<Texture2D>($"res://Assets/{cardId}.jpg");
		}
		catch (Exception)
		{
			sprite.Texture = GD.Load<Texture2D>(CARD_BACK_ASSET_PATH);
		}
	}

	public void SetStatsVisibility(bool visible)
	{
		_atkLabel.Visible = visible;
		_defLabel.Visible = visible;
		_slashLabel.Visible = visible;
	}

	public void SetStatsColorForPosition(bool isAttackPosition)
	{
		_slashLabel.Modulate = Gray;

		if (isAttackPosition)
		{
			_atkLabel.Modulate = White;
			_defLabel.Modulate = Gray;
		}
		else
		{
			_atkLabel.Modulate = Gray;
			_defLabel.Modulate = White;
		}
	}

	#endregion
}
