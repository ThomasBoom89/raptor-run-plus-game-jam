using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Godot;
using RaptorRunPlus.scene.ui.gameover.highscore_board;

namespace RaptorRunPlus.scene.service;

public class Highscore
{
    private const string Supersecretpassword = "TopSecretPassword";
    private const string UserScoresCfgPath = "user://scores.cfg";

    private HighscoreEntry[] _highscoreEntries = null;

    public void InsertEntry(string name, int score)
    {
        List<HighscoreEntry> highscoreEntries = new List<HighscoreEntry>();
        int rank = GetRankByScore(score);
        for (int i = 1; i <= 10; i++)
        {
            if (i < rank)
            {
                highscoreEntries.Add(_highscoreEntries[i - 1]);
            }
            else if (i == rank)
            {
                HighscoreEntry highscoreEntry = new HighscoreEntry();
                highscoreEntry.Name = name;
                highscoreEntry.Rank = rank;
                highscoreEntry.Score = score;
                highscoreEntries.Add(highscoreEntry);
            }
            else
            {
                HighscoreEntry highscoreEntry = _highscoreEntries[i - 2];
                highscoreEntry.Rank = i;
                highscoreEntries.Add(highscoreEntry);
            }
        }

        _highscoreEntries = highscoreEntries.ToArray();
        _saveHighscoreEntries();
    }

    public int GetRankByScore(int score)
    {
        int iter = 1;
        foreach (HighscoreEntry highscoreEntry in _highscoreEntries)
        {
            if (score > highscoreEntry.Score)
            {
                return iter;
            }

            iter++;
        }

        return 0;
    }

    public HighscoreEntry[] GetEntries()
    {
        if (_highscoreEntries == null)
        {
            _createEntriesFromConfigFile();
        }

        return _highscoreEntries;
    }

    private void _createEntriesFromConfigFile()
    {
        try
        {
            ConfigFile config = _GetConfigFile();
            List<HighscoreEntry> highscoreEntries = new List<HighscoreEntry>();
            foreach (string player in config.GetSections())
            {
                HighscoreEntry highscoreEntry = new HighscoreEntry();
                highscoreEntry.Name = (string)config.GetValue(player, "name");

                string pattern = @"\d+";
                Regex regex = new Regex(pattern);

                highscoreEntry.Rank = Int16.Parse(regex.Match(player).Value);
                highscoreEntry.Score = (int)config.GetValue(player, "score");

                highscoreEntries.Add(highscoreEntry);
            }

            _highscoreEntries = highscoreEntries.ToArray();
        }
        catch (Exception)
        {
            _initializeEntries();
            _saveConfigFile(new ConfigFile());
            _saveHighscoreEntries();
        }
    }

    private ConfigFile _GetConfigFile()
    {
        ConfigFile config = new ConfigFile();

        // Error err = config.Load(UserScoresCfgPath);
        Error err = config.LoadEncryptedPass(UserScoresCfgPath, Supersecretpassword);

        if (err != Error.Ok)
        {
            throw new Exception();
        }

        return config;
    }

    private void _initializeEntries()
    {
        List<HighscoreEntry> highscoreEntries = new List<HighscoreEntry>();
        for (int i = 1; i <= 10; i++)
        {
            HighscoreEntry highscoreEntry = new HighscoreEntry();
            highscoreEntry.Rank = i;
            highscoreEntry.Score = 0;
            highscoreEntry.Name = "";
            highscoreEntries.Add(highscoreEntry);
        }

        _highscoreEntries = highscoreEntries.ToArray();
    }

    private void _saveHighscoreEntries()
    {
        ConfigFile config = _GetConfigFile();
        foreach (HighscoreEntry highscoreEntry in _highscoreEntries)
        {
            config.SetValue("Rank" + highscoreEntry.Rank, "name", highscoreEntry.Name);
            config.SetValue("Rank" + highscoreEntry.Rank, "score", highscoreEntry.Score);
        }

        _saveConfigFile(config);
    }

    private static void _saveConfigFile(ConfigFile config)
    {
        // config.Save(UserScoresCfgPath);
        config.SaveEncryptedPass(UserScoresCfgPath, Supersecretpassword);
    }
}