using TMPro;
using UnityEngine;

[DefaultExecutionOrder(1000)]
[RequireComponent(typeof(TMP_Text))]
public class BestScoreText : MonoBehaviour
{
    private TMP_Text _text;
    
    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
        var (playerName, score) = PersistenceManager.Instance;
        Set(playerName, score);
    }
    
    private void OnEnable()
    {
        PersistenceManager.NewBestScore += Set;
    }

    private void OnDisable()
    {
        PersistenceManager.NewBestScore -= Set;
    }
    
    private void Set(string playerName, int score)
    {
        if (playerName is null)
        {
            Debug.LogWarning("Player's name was null. Cannot update Best Score text");
            return;
        }

        _text.text = $"Best Score : {playerName} : {score}";
    }
}