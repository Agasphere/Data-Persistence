using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

[DefaultExecutionOrder(1000)]
public class MenuUIHandler : MonoBehaviour
{
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private TMP_Text _warningText;
    
    [SerializeField] private float _warningTextDisplayTime = 1;

    private void Awake()
    {
        _inputField = GetComponentInChildren<TMP_InputField>();
    }

    public void StartSession()
    {
        var playerName = _inputField.text;
        if (playerName == string.Empty)
        {
            StopCoroutine(nameof(DisplayWarning));
            StartCoroutine(DisplayWarning(_warningText, _warningTextDisplayTime));
            return;
        }

        PersistenceManager.Instance.RegisterPlayer(playerName);
        SceneManager.LoadScene(1);
    }

    private static IEnumerator DisplayWarning(Graphic i, float t)
    {
        var color = i.color;
        color.a = 1;

        while (color.a > 0.0f)
        {
            color.a -= Time.deltaTime / t;
            i.color = color;
            yield return null;
        }
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