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
        if (Input.GetKey(KeyCode.Space))
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
}
