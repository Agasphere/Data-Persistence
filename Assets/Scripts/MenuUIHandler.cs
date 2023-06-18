using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

[DefaultExecutionOrder(1000)]
public class MenuUIHandler : MonoBehaviour
{
    [SerializeField] private TMP_InputField _inputField;

    private void Awake()
    {
        _inputField = GetComponentInChildren<TMP_InputField>();
    }

    public void StartSession()
    {
        var playerName = _inputField.text;
        if (playerName == string.Empty) return;

        PersistenceManager.Instance.RegisterPlayer(playerName);
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}