using TMPro;
using UnityEngine;
using System;

public class MinigameManager : MonoBehaviour
{
    public static MinigameManager Instance;

    [SerializeField] private TextMeshPro scoreText;
    [SerializeField] private TextMeshPro timeText;
    [SerializeField] private float timeEasy = 60f;  // Total countdown time in seconds
    [SerializeField] private float timeMedium = 45f;  // Total countdown time in seconds
    [SerializeField] private float timeHard = 30f;  // Total countdown time in seconds
    [SerializeField] private int rowEasy = 4;
    [SerializeField] private int rowMedium = 5;
    [SerializeField] private int rowHard = 6;
    [SerializeField] private int columnEasy = 7;
    [SerializeField] private int columnMedium = 10;
    [SerializeField] private int columnHard = 13;
    [SerializeField] private MinigameMenu menu;
    [SerializeField] private TargetSpawner spawner;

    private int score;
    private float timeRemaining;
    private bool timerRunning;

    public event Action OnTimerEnded;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        timerRunning = false;
        timeRemaining = 0;
        UpdateTimeText();
        score = 0;
        scoreText.text = score.ToString();

        ShowMenu(true);
    }

    private void Update()
    {
        if (timerRunning)
        {
            Countdown();
        }
    }

    public void ShowMenu(bool newGame)
    {
        MinigameMenu menuObject = Instantiate(menu);
        menuObject.SetTitle(newGame);
    }

    public void StartGame(GameDifficulty gameDifficulty)
    {
        score = 0;
        scoreText.text = score.ToString();

        switch (gameDifficulty)
        {
            case GameDifficulty.Easy:
                timeRemaining = timeEasy;
                spawner.SpawnTargets(rowEasy, columnEasy);
                break;
            case GameDifficulty.Medium:
                timeRemaining = timeMedium;
                spawner.SpawnTargets(rowMedium, columnMedium);
                break;
            case GameDifficulty.Hard:
                timeRemaining = timeHard;
                spawner.SpawnTargets(rowHard, columnHard);
                break;
        }

        timerRunning = true;
        UpdateTimeText();

    }

    public void IncreaseScoreBy(int score)
    {
        // Only add score if the timer is still running
        if (timerRunning)
        {
            this.score += score;
            scoreText.text = this.score.ToString();
        }
    }

    private void Countdown()
    {
        timeRemaining -= Time.deltaTime;

        // Check if time has run out
        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            timerRunning = false;

            OnTimerEnded?.Invoke();
        }

        UpdateTimeText();
    }

    private void UpdateTimeText()
    {
        // Format time as minutes:seconds and update text
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timeText.text = $"{minutes:00}:{seconds:00}";
    }
}