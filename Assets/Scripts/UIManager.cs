using System;
using UnityEngine;
using UnityEngine.UI;

public enum GameScreens
{
    OnStart,
    Gameplay,
    InShop,
    GameOver,
    GamePause
}

public class UIManager : MonoBehaviour
{
    public GameObject[] canvases;
    public Transform leaderboard;
    public GameObject recordPrefab;
    public Text playerNameInputField;
    public Text highestScoreText;
    public Text playerScoreText;
    public Text livesCount;
    public Text coinsCount;
    public Text gameTime;
    public Text shopLivesCount;
    public Text shopCoinsCount;
    public Button buyButton;
    [SerializeField]
    private GameScreens gameScreen;
    public GameScreens GameScreen
    {
        set
        {
            gameScreen = value;
            OnGameScreenChanged();
        }

        get
        {
            return gameScreen;
        }
    }
    public Action<GameScreens> gameScreenChanged;
    public static UIManager Instance;

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

    void Start()
    {
        GameManager.Instance.gameStateChanged += OnGameStateChanged;
        Player.Instance.playerLivesChanged += OnPlayerLivesChanged;
        Player.Instance.playerCollectedCoin += OnPlayerCollectCoins;
    }

    private void Update()
    {
        gameTime.text = ((int)GameManager.Instance.countDownTimer).ToString();
    }

    private void OnGameStateChanged(GameStates gameState)
    {
        if (gameState == GameStates.GameStart)
        {
            GameScreen = GameScreens.OnStart;
        }
        else if (gameState == GameStates.InGame)
        {
            GameScreen = GameScreens.Gameplay;
        }
        // gameState == GameEnds
        else
        {
            if (GameManager.Instance.gameWin)
            {
                GameScreen = GameScreens.InShop;
            }
            else
            {
                GameScreen = GameScreens.GameOver;
            }
        }
    }

    private void OnGameScreenChanged()
    {
        gameScreenChanged?.Invoke(GameScreen);
        if (GameScreen == GameScreens.OnStart)
        {
            ShowCanvas(0);
            UpdateLeaderboardUIData();
            PopulateLeaderData();
        }
        else if (GameScreen == GameScreens.Gameplay)
        {
            ShowCanvas(1);
        }
        else if (GameScreen == GameScreens.InShop)
        {
            ShowCanvas(2);
            buyButton.interactable = Player.Instance.CoinsCollected >= Player.Instance.maxCoins &&
                Player.Instance.LivesCount < Player.Instance.maxLives;
            OnPlayerLivesChanged(Player.Instance.LivesCount);
            OnPlayerCollectCoins(Player.Instance.CoinsCollected);
        }
        else if (GameScreen == GameScreens.GameOver)
        {
            ShowCanvas(3);
            if (GameManager.Instance.leaderboard.leaderboardData.records.Count == 0)
            {
                highestScoreText.text = "0";
            }
            else
            {
                highestScoreText.text = GameManager.Instance.leaderboard
                    .leaderboardData.records[0].coins > Player.Instance.CoinsCollected ?
                    GameManager.Instance.leaderboard
                    .leaderboardData.records[0].coins.ToString() :
                    Player.Instance.CoinsCollected.ToString();
            }
            playerScoreText.text = Player.Instance.CoinsCollected.ToString();
            playerNameInputField.text = "";
        }
        // GameState == game pause
        else
        {
            ShowCanvas(4);
        }
    }

    private void UpdateLeaderboardUIData()
    {
        for (int i = 2; i < leaderboard.childCount; i++)
        {
            Destroy(leaderboard.GetChild(i).gameObject);
        }
    }

    private void PopulateLeaderData()
    {
        var records = GameManager.Instance.leaderboard.leaderboardData.records;
        for (int i = 0; i < records.Count; i++)
        {
            // check that the record is not empty
            if (!records[i].name.Equals(""))
            {
                var recordGO = Instantiate(recordPrefab);
                recordGO.transform.parent = leaderboard;
                recordGO.transform.localScale /= recordGO.transform.localScale.x;
                // populate name
                recordGO.transform.GetChild(0).GetComponent<Text>().text = records[i].name;
                // populate score
                recordGO.transform.GetChild(1).GetComponent<Text>().text = records[i].coins.ToString();
            }
        }
    }

    private void OnPlayerLivesChanged(int livesCount)
    {
        this.livesCount.text = livesCount.ToString();
        shopLivesCount.text = livesCount.ToString();
    }

    private void OnPlayerCollectCoins(int coinsCount)
    {
        this.coinsCount.text = coinsCount.ToString();
        shopCoinsCount.text = coinsCount.ToString();
    }

    private void ShowCanvas(int canvasIndex)
    {
        for (int i = 0; i < canvases.Length; i++)
        {
            var active = (i == canvasIndex) ? true : false;
            canvases[i].SetActive(active);
        }
    }

    public void StartGameButtonPressed()
    {
        GameManager.Instance.GameState = GameStates.InGame;
    }

    public void PauseButtonPressed(bool pause)
    {
        if (pause)
        {

            GameScreen = GameScreens.GamePause;
        }
        else
        {
            GameScreen = GameScreens.Gameplay;
        }
    }

    public void BuyButtonPressed()
    {
        if (Player.Instance.CoinsCollected >= 10)
        {
            Player.Instance.LivesCount++;
            Player.Instance.CoinsCollected -= 10;
            if (Player.Instance.CoinsCollected < 10)
            {
                buyButton.interactable = false;
            }
        }
    }

    public void ReturnToGameButtonPressed()
    {
        GameScreen = GameScreens.GameOver;
    }

    public void SaveDataButtonPressed()
    {
        Record record = new Record();
        record.name = playerNameInputField.text;
        record.coins = Player.Instance.CoinsCollected;
        Player.Instance.CoinsCollected = 0;
        GameManager.Instance.leaderboard.AddToRecords(record);
        GameScreen = GameScreens.OnStart;
    }
}
