using System;
using System.IO;
using UnityEngine;

public class PersistenceManager : MonoBehaviour
{
    public static PersistenceManager Instance { get; private set; }

    [Serializable]
    private class PlayerData
    {
        public string Name;
        public int BestScore;
    }

    public string PlayerName;
    private int _playerBestScore;
    private static string FilePath => Path.Combine(Application.persistentDataPath, "/savefile.json");

    public static Action<string, int> NewBestScore;
    public static Action<string> LastPlayerLoaded;

    private void Awake()
    {
        if (Instance)
            Destroy(gameObject);
        else
            Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        MainManager.ScoreIncreased += OnScoreIncreased;
    }

    private void OnDisable()
    {
        MainManager.ScoreIncreased -= OnScoreIncreased;
    }

    private void OnScoreIncreased(int newScore)
    {
        var previousBestScore = _playerBestScore;
        _playerBestScore = Mathf.Max(newScore, _playerBestScore);

        if (_playerBestScore > previousBestScore)
            NewBestScore?.Invoke(PlayerName, _playerBestScore);
    }

    public bool LoadPlayerData()
    {
        var filePath = FilePath;

        if (!File.Exists(filePath))
            return false;
        var json = File.ReadAllText(filePath);
        var data = JsonUtility.FromJson<PlayerData>(json);

        PlayerName = data.Name;
        _playerBestScore = data.BestScore;

        LastPlayerLoaded?.Invoke(PlayerName);
        NewBestScore?.Invoke(PlayerName, _playerBestScore);

        return true;
    }

    public void SavePlayerData()
    {
        var data = new PlayerData
        {
            Name = PlayerName,
            BestScore = _playerBestScore
        };
        var json = JsonUtility.ToJson(data);
        File.WriteAllText(FilePath, json);
    }
}