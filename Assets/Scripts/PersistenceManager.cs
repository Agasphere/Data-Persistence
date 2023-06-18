using System;
using System.Collections.Generic;
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

    private static readonly PlayerData NullData = new() { Name = string.Empty };

    private readonly Queue<PlayerData> _dataToSave = new ();

    private static string FilePath => Path.Combine(Application.persistentDataPath, "/savefile.json");

    public static Action<string, int> NewBestScore;

    private void Awake()
    {
        if (Instance)
            Destroy(gameObject);
        else
            Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadPlayerData();
    }

    private void OnEnable()
    {
        MainManager.ScoreIncreased += OnScoreIncreased;
        MainManager.GameEnded += SavePlayerData;
    }

    private void OnDisable()
    {
        MainManager.ScoreIncreased -= OnScoreIncreased;
        MainManager.GameEnded -= SavePlayerData;
    }

    private void OnScoreIncreased(int newScore)
    {
        var currentScore = _dataToSave.Peek().BestScore;
        if (newScore <= currentScore) return;

        if (_dataToSave.Count > 1) 
            _dataToSave.Dequeue();
        
        var data = _dataToSave.Peek();
        data.BestScore = newScore;
        NewBestScore?.Invoke(data.Name, data.BestScore);
    }

    public bool LoadPlayerData()
    {
        var filePath = FilePath;

        if (!File.Exists(filePath))
        {
            _dataToSave.Enqueue(NullData);
            return false;
        }

        var json = File.ReadAllText(filePath);
        var playerData = JsonUtility.FromJson<PlayerData>(json);
        _dataToSave.Enqueue(playerData);

        return true;
    }

    public void SavePlayerData()
    {
        var json = JsonUtility.ToJson(_dataToSave.Peek());
        File.WriteAllText(FilePath, json);
    }

    public void RegisterPlayer(string newPlayer)
    {
        var lastPlayer = _dataToSave.Peek();
        if (newPlayer is null || lastPlayer.Name == newPlayer) 
            return;
        
        if (lastPlayer.Equals(NullData)) 
            _dataToSave.Dequeue();

        _dataToSave.Enqueue(new PlayerData
        {
            Name = newPlayer
        });
    }

    public void Deconstruct(out string playerName, out int playerBestScore)
    {
        var data = _dataToSave.Peek();
        playerName = data.Name;
        playerBestScore = data.BestScore;
    }
}