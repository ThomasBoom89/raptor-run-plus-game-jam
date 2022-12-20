namespace RaptorRunPlus.scene.service;

public class GameState
{
    private int _score = 0;

    public void SetScore(int score)
    {
        _score = score;
    }

    public int GetScore()
    {
        return _score;
    }
}