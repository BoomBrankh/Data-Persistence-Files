using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;

    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;

    public static int highScore;
    private bool beatenHighScore = false;
    public static string highScoreName;
    public Text highScoreBoard;

    private bool m_GameOver = false;

    public void Awake()
    {
        // Destroys this instance of Main manager if it already exists
        //if (Instance != null)
       // {
        //    Destroy(gameObject);
          //  return;
        //}

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadHighScore();
        highScoreBoard.text = "Best Score: " + highScoreName + " : " + highScore;
    }

    [System.Serializable]
    class SaveData
    {
        public int highScore;
        public string userName;
    }

    // Start is called before the first frame update
    void OnLevelWasLoaded()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_GameOver = false;
                beatenHighScore = false;
                m_Started = false;
                SceneManager.LoadScene(0);
                Destroy(gameObject);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
        if(highScore <= m_Points)
        {
            beatenHighScore = true;
            highScoreBoard.text = "Best Score: " + NameManager.playerName + " : " + m_Points;
        }
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        if(beatenHighScore)
        {
            SaveHighScore();
        }
        
    }

    public void SaveHighScore()
    {
        SaveData data = new SaveData();
        data.highScore = m_Points;
        data.userName = NameManager.playerName;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public static void LoadHighScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            highScoreName = data.userName;
            highScore = data.highScore;
        }
    }
}
