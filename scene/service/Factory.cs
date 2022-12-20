namespace RaptorRunPlus.scene.service;

public class Factory
{
    private GameState _gameState = null;

    private Highscore _highscore = null;

    public GameState GetGameState()
    {
        if (_gameState == null)
        {
            _gameState = new GameState();
        }

        return _gameState;
    }

    public Highscore GetHighscore()
    {
        if (_highscore == null)
        {
            _highscore = new Highscore();
        }

        return _highscore;
    }
}