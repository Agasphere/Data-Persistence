using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_InputField))]
public class NameInputText : MonoBehaviour
{
    private TMP_InputField _inputField;

    private void Awake()
    {
        _inputField = GetComponent<TMP_InputField>();
    }

    private void OnEnable()
    {
        PersistenceManager.LastPlayerLoaded += Set;
    }

    private void OnDisable()
    {
        PersistenceManager.LastPlayerLoaded -= Set;
    }
    
    private void Set(string playerName)
    {
        if (playerName is null)
        {
            Debug.LogWarning("Player's name was null. Name input field text will stay empty");
            return;
        }

        _inputField.text = playerName;
    }
}