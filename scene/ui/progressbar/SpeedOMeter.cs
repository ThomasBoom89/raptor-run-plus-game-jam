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
        // GD.Print(red, green, "MinValue: ", MinValue, "MaxValue: ", MaxValue, "Value: ", Value);
        SelfModulate = new Color((float)red, (float)green, 0, 1);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
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
        // 300 -> 0
        // 900 -> 1
        return (Value - MinValue) * (1 / (MaxValue - MinValue));
    }
}