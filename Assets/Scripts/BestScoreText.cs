using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class BestScoreText : MonoBehaviour
{
    private TMP_Text _text;
    
    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }
    
    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }
    
    private void Set(string playerName, int score)
    {
        if (playerName is null)
        {
            Debug.LogWarning("Player's name was null. Best Score text will stay empty");
            return;
        }

        _text.text = $"Best Score : {playerName} : {score}";
    }
}