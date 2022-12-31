using Godot;

namespace RaptorRunPlus.scene.ui.progressbar;

public partial class SpeedOMeter : ProgressBar
{
    private Game _game;

    public override void _Ready()
    {
        _game = GetNode<Game>("/root/World");
        MinValue = Game.MinWorldSpeed;
        MaxValue = Game.MaxWorldSpeed;
        Value = 300;
        CalculateColor();
    }

    private void CalculateColor()
    {
        double red = RangeLerp();
        double green = 1 - RangeLerp();
        SelfModulate = new Color((float)red, (float)green, 0, 1);
    }

    public override void _Process(double delta)
    {
        if (_game.WorldSpeed != Value)
        {
            Value = _game.WorldSpeed;
            CalculateColor();
        }
    }

    private double RangeLerp()
    {
        return (Value - MinValue) * (1 / (MaxValue - MinValue));
    }
}