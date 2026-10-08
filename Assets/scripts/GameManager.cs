using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public TMP_Text timerText;
    public TMP_Text resultText;
    public TMP_Text levelText;
    public TMP_Text scoreText;

    public Transform player;

    public GameObject startPanel;
    public GameObject restartButton;

    float timeLeft;

    int currentLevel;
    int levelTarget;

    int bestScore;

    bool gameOver = false;

    public bool gameStarted = false;

    void Start()
    {
        currentLevel = PlayerPrefs.GetInt("Level", 1);
        bestScore = PlayerPrefs.GetInt("BestScore", 0);

        bool skipStartPanel =
            PlayerPrefs.GetInt("SkipStartPanel", 0) == 1;

        levelTarget = currentLevel * 10;
        timeLeft = levelTarget;

        levelText.text = "LEVEL: " + currentLevel;
        scoreText.text = "BEST SCORE: " + bestScore;

        resultText.text = "";
        resultText.gameObject.SetActive(false);

        if (skipStartPanel)
        {
            PlayerPrefs.SetInt("SkipStartPanel", 0);
            PlayerPrefs.Save();

            gameStarted = true;

            startPanel.SetActive(false);

            timerText.gameObject.SetActive(true);
            levelText.gameObject.SetActive(true);
            scoreText.gameObject.SetActive(true);
            restartButton.SetActive(true);

            Time.timeScale = 1f;
        }
        else
        {
            Time.timeScale = 0f;

            startPanel.SetActive(true);

            timerText.gameObject.SetActive(false);
            levelText.gameObject.SetActive(false);
            scoreText.gameObject.SetActive(false);
            restartButton.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayerPrefs.SetInt("SkipStartPanel", 1);
            PlayerPrefs.Save();

            Time.timeScale = 1f;

            SceneManager.LoadScene(
                SceneManager.GetActiveScene().buildIndex
            );
        }

        if (!gameStarted)
            return;

        if (gameOver)
            return;

        timeLeft -= Time.deltaTime;

        int minutes = Mathf.FloorToInt(timeLeft / 60);
        int seconds = Mathf.FloorToInt(timeLeft % 60);

        timerText.text =
            string.Format("{0:00}:{1:00}", minutes, seconds);

        int currentScore =
            (currentLevel * 10) -
            Mathf.CeilToInt(timeLeft);

        if (currentScore > bestScore)
        {
            bestScore = currentScore;

            PlayerPrefs.SetInt(
                "BestScore",
                bestScore
            );

            PlayerPrefs.Save();

            scoreText.text =
                "BEST SCORE: " + bestScore;
        }

        if (player.position.y < -1f)
        {
            LoseGame();
        }

        if (timeLeft <= 0)
        {
            WinGame();
        }
    }

    public void StartGame()
    {
        gameStarted = true;

        startPanel.SetActive(false);

        timerText.gameObject.SetActive(true);
        levelText.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(true);
        restartButton.SetActive(true);

        Time.timeScale = 1f;
    }

    void WinGame()
    {
        resultText.gameObject.SetActive(true);
        resultText.text = "YOU WON!";

        gameOver = true;

        bestScore = currentLevel * 10;

        PlayerPrefs.SetInt(
            "BestScore",
            bestScore
        );

        currentLevel++;

        PlayerPrefs.SetInt(
            "Level",
            currentLevel
        );

        PlayerPrefs.Save();

        scoreText.text =
            "BEST SCORE: " + bestScore;

        Time.timeScale = 0f;
    }

    public void LoseGame()
    {
        resultText.gameObject.SetActive(true);
        resultText.text = "YOU LOST!";

        gameOver = true;

        Time.timeScale = 0f;
    }

    public void StartFromScratch()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}