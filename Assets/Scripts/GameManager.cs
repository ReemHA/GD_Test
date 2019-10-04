using UnityEngine;
using System;

public enum GameStates
{
    GameStart,
    InGame,
    GameEnds
}

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameStates gameState;
    public GameStates GameState
    {
        set
        {
            gameState = value;
            OnGameStateChange();
        }

        get
        {
            return gameState;
        }
    }
    public Action<GameStates> gameStateChanged;
    public int gameTime = 60;
    public float countDownTimer;
    public bool gameWin = false;
    public static GameManager Instance;
    private bool doOnce = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        GameState = GameStates.GameStart;
        UIManager.Instance.gameScreenChanged += OnGameScreenChange;
        Player.Instance.playerLivesChanged += OnPlayerLivesChanged;
        countDownTimer = gameTime;
    }

    private void Update()
    {
        if (countDownTimer > 0)
        {
            countDownTimer -= Time.deltaTime;
        }
        else
        {
            if (!doOnce)
            {
                if (Player.Instance.LivesCount > 0)
                {
                    gameWin = true;
                }
                GameState = GameStates.GameEnds;
                doOnce = true;
            }
        }
    }

    private void OnGameStateChange()
    {
        gameStateChanged?.Invoke(gameState);
        if (GameState == GameStates.GameStart)
        {
            Time.timeScale = 0;
        }
        else if (GameState == GameStates.InGame)
        {
            Time.timeScale = 1;
            countDownTimer = gameTime;
            doOnce = false;
            gameWin = false;
        }
        // GameState == GameEnds
        else
        {
            Time.timeScale = 0;
        }
    }

    private void OnGameScreenChange(GameScreens gameScreen)
    {
        switch (gameScreen)
        {
            case GameScreens.OnStart:
                Time.timeScale = 0;
                break;
            case GameScreens.Gameplay:
                Time.timeScale = 1;
                break;
            case GameScreens.InShop:
                Time.timeScale = 0;
                break;
            case GameScreens.GameOver:
                Time.timeScale = 0;
                break;
            case GameScreens.GamePause:
                Time.timeScale = 0;

                break;
            default:
                break;
        }

    }

    private void OnPlayerLivesChanged(int lives)
    {
        if (lives <= 0)
        {
            GameState = GameStates.GameEnds;
            gameWin = false;
        }
    }
}
