using Godot;

namespace RaptorRunPlus.scene.ui.gameover.highscore_board;

public partial class Entry : HBoxContainer
{
    private Label _rankLabel;
    private Label _nameLabel;
    private Label _scoreLabel;

    private HighscoreEntry _highscoreEntry;

    public override void _Ready()
    {
        _rankLabel = GetNode<Label>("%Rank");
        _nameLabel = GetNode<Label>("%Name");
        _scoreLabel = GetNode<Label>("%Score");
        _setLabelText();
    }

    public void SetHighscoreEntry(HighscoreEntry highscoreEntry)
    {
        _highscoreEntry = highscoreEntry;
    }

    private void _setLabelText()
    {
        _rankLabel.Text = _highscoreEntry.Rank + ".";
        _nameLabel.Text = _highscoreEntry.Name;
        _scoreLabel.Text = _highscoreEntry.Score.ToString();
    }
}