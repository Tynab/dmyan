using Godot;
using System.Threading.Tasks;
using YANLib;
using static Godot.ResourceLoader;
using static System.Threading.Tasks.Task;
using static YANLib.YANMath;

namespace DMYAN.Scripts.ProfileStack;

internal partial class Profile : DMYANNode2D
{
    internal void Init(string name, float lp = 2_000)
    {
        _name.Text = name;
        _lifePoint.Text = lp.ToString();
        _health.MaxValue = lp;
        _avatar.Texture = Load<Texture2D>($"res://Assets/Avatars/{name}.jpg");
    }

    internal Vector2 GetAvatarPosition()
    {
        var position = _avatar.GlobalPosition;

        return new Vector2(position.X + _avatar.Size.X / 2, position.Y + _avatar.Size.Y / 2);
    }

    internal async Task UpdateLifePoint(int lpUpdate)
    {
        if (lpUpdate < 0)
        {
            while (lpUpdate <= -10)
            {
                lpUpdate += 10;

                var lp = Max(_lifePoint.Text.Parse<int>() - 10, 0);

                _lifePoint.Text = lp.ToString();
                _health.Value = lp;

                if (lp is 0)
                {
                    break;
                }

                await Delay(1);
            }

            if (lpUpdate < 0)
            {
                var lp = Max(_lifePoint.Text.Parse<int>() + lpUpdate, 0);

                _lifePoint.Text = lp.ToString();
                _health.Value = lp;
            }
        }
        else
        {
            while (lpUpdate >= 10)
            {
                lpUpdate -= 10;

                var lp = _lifePoint.Text.Parse<int>() + 10;

                _lifePoint.Text = lp.ToString();
                _health.Value = lp;
            }

            if (lpUpdate > 0)
            {
                var lp = _lifePoint.Text.Parse<int>() + lpUpdate;

                _lifePoint.Text = lp.ToString();
                _health.Value = lp;
            }
        }
    }
}
