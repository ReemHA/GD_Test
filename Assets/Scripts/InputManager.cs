using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{
    public Action<bool> runPressed;
    public Action jumpPressed;
    public static InputManager Instance;

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
        UIManager.Instance.gameScreenChanged += OnGameScreenChange;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            OnRunPressed(true);
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            OnRunPressed(false);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnJumpPressed();
        }
    }

    private void OnJumpPressed()
    {
        jumpPressed?.Invoke();
    }

    private void OnRunPressed(bool buttonPressed)
    {
        runPressed?.Invoke(buttonPressed);
    }

    private void OnGameScreenChange(GameScreens gameScreen)
    {
        switch (gameScreen)
        {
            case GameScreens.OnStart:
                enabled = false;
                break;
            case GameScreens.Gameplay:
                enabled = true;
                break;
            case GameScreens.InShop:
                enabled = false;
                break;
            case GameScreens.GameOver:
                enabled = false;
                break;
            case GameScreens.GamePause:
                enabled = false;
                break;
            default:
                break;
        }

    }

}
