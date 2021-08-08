using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class NameManager : MonoBehaviour
{
    public TMP_InputField inputPlayerName;
    public TextMeshProUGUI currentHighScore;
    public static string playerName;


    public void Awake()
    {
        MainManager.LoadHighScore();
        currentHighScore.text = "Best Score: " + MainManager.highScoreName + " : " + MainManager.highScore;
    }

    public void StartNew()
    {
        playerName = inputPlayerName.text;
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
