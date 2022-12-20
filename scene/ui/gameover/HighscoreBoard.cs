using Godot;
using RaptorRunPlus.scene.service;
using RaptorRunPlus.scene.ui.gameover.highscore_board;

namespace RaptorRunPlus.scene.ui.gameover;

public partial class HighscoreBoard : PanelContainer
{
    private Highscore _highscore;

    private VBoxContainer _highscoreEntryContainer;

    private PackedScene _entry;

    public HighscoreBoard()
    {
        _highscore = DIContainer.GetFactory().GetHighscore();
    }

    public override void _Ready()
    {
        _highscoreEntryContainer = GetNode<VBoxContainer>("VBoxContainer/VBoxContainer");
        _entry = GD.Load<PackedScene>("res://scene/ui/gameover/highscore_board/entry.tscn");
        _createHighscoreEntries();
    }

    private void _createHighscoreEntries()
    {
        foreach (HighscoreEntry highscoreEntry in _highscore.GetEntries())
        {
            Entry entryInstance = _entry.Instantiate<Entry>();
            entryInstance.SetHighscoreEntry(highscoreEntry);
            _highscoreEntryContainer.AddChild(entryInstance);
        }
    }

    public void Refresh()
    {
        foreach (Node child in _highscoreEntryContainer.GetChildren())
        {
            child.QueueFree();
        }

        _createHighscoreEntries();
    }
}