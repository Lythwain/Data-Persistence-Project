using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text TopScoreText;
    public Text GameOverMessage;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        DisplayScore();
        CreateBricks();
    }
    private void CreateBricks()
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
                DataManager.Instance.brickCount++;
            }
        }
    }
    private void Update()
    {
        if(!m_GameOver && (DataManager.Instance.brickCount == 0))
        {
            CreateBricks();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0,LoadSceneMode.Single);
            return;
        }
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
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
    void DisplayScore()
    {
        ScoreText.text = $"Player {DataManager.Instance.playerName} Score : {m_Points}";
        GetHighScore();
    }
    void AddPoint(int point)
    {
        m_Points += point;
        DisplayScore();
    }
    public int GetHighScore()
    {
        try
        {
            string path = Application.dataPath + "/HighScore.json";
            string json = File.ReadAllText(path);
            DataManager.Instance.data = JsonUtility.FromJson<SaveData>(json);
            TopScoreText.text = $"Best Score : {DataManager.Instance.data.name} : {DataManager.Instance.data.score}";
        }
        catch(FileNotFoundException)
        {
            DataManager.Instance.data.score = m_Points;
            DataManager.Instance.data.name = DataManager.Instance.playerName;
            string json = JsonUtility.ToJson(DataManager.Instance.data);
            File.WriteAllText(Application.dataPath + "/HighScore.json", json);
        }
        return DataManager.Instance.data.score;
    }

    public void SetHighScore()
    {
        int high = GetHighScore();
        if(high > m_Points)
        {
            //this is not the high score so exit
            return;
        }
        DataManager.Instance.data.score = m_Points;
        DataManager.Instance.data.name = DataManager.Instance.playerName;
        string json = JsonUtility.ToJson(DataManager.Instance.data);

        File.WriteAllText(Application.dataPath + "/HighScore.json", json);
    }
    public void GameOver()
    {
        SetHighScore();
        m_GameOver = true;
        GameOverMessage.text = "GAME OVER\nPress Space to Restart.\nHit Esc to return to main menu.";
        GameOverText.SetActive(true);
    }
}
