using DMYAN.Scripts.GameManagerStack;
using Godot;
using System.Threading.Tasks;
using static DMYAN.Scripts.Common.Constant;
using static Godot.Tween.EaseType;
using static Godot.Tween.TransitionType;
using static Godot.Vector2;

namespace DMYAN.Scripts.Controls;

internal partial class PhaseButton : Button
{
    internal protected GameManager _gameManager;

    internal protected bool IsClicked { get; set; } = false;

    public override void _Ready()
    {
        var main = GetTree().Root.GetNode(MAIN_NODE);

        _gameManager = main.GetNode<GameManager>(nameof(GameManager));

        ButtonDown += OnButtonDown;
    }

    private void OnButtonDown() => AddThemeFontSizeOverride(FONT_SIZE_PROPERTY, BUTTON_DOWN_FONT_SIZE);

    internal void ChangeStatus(bool isEnable)
    {
        Disabled = !isEnable;

        if (isEnable)
        {
            AddThemeFontSizeOverride(FONT_SIZE_PROPERTY, BUTTON_UP_FONT_SIZE);
        }
        else
        {
            AddThemeFontSizeOverride(FONT_SIZE_PROPERTY, BUTTON_DOWN_FONT_SIZE);
        }
    }

    internal async Task In()
    {
        ZIndex = 0;
        Scale = new Vector2(.2f, 1);
        IsClicked = false;

        ChangeStatus(true);
        Show();

        _ = await ToSignal(GetTree().CreateTween().SetTrans(Linear).SetEase(InOut).TweenProperty(this, SCALE_NODE_PATH, One, DEFAULT_ANIMATION_SPEED), FINISHED_SIGNAL);
    }

    internal async Task Out()
    {
        ZIndex = 1;
        Scale = One;
        IsClicked = true;

        ChangeStatus(false);

        _ = await ToSignal(GetTree().CreateTween().SetTrans(Linear).SetEase(InOut).TweenProperty(this, SCALE_NODE_PATH, new Vector2(.2f, 1), DEFAULT_ANIMATION_SPEED), FINISHED_SIGNAL);

        Hide();
    }
}
