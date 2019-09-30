using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{
    public Action<bool> playerRuns;
    public Action playerJumps;
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

    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            OnPlayerRuns(true);
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            OnPlayerRuns(false);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            OnPlayerJumps();
        }
    }

    private void OnPlayerJumps()
    {
        playerJumps?.Invoke();
    }

    private void OnPlayerRuns(bool buttonPressed)
    {
        playerRuns?.Invoke(buttonPressed);
    }
}
