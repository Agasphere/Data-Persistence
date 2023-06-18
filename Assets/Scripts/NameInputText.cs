using TMPro;
using UnityEngine;

[DefaultExecutionOrder(1000)]
[RequireComponent(typeof(TMP_InputField))]
public class NameInputText : MonoBehaviour
{
    private TMP_InputField _inputField;

    private void Awake()
    {
        _inputField = GetComponent<TMP_InputField>();
        var (playerName, _) = PersistenceManager.Instance;
        Set(playerName);
    }

    private void Set(string playerName)
    {
        if (playerName is null)
        {
            Debug.LogWarning("Player's name was null. Cannot update Name input field text");
            return;
        }

        _inputField.text = playerName;
    }
}