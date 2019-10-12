using System;
using UnityEngine;

public enum SoundNames
{
    PlayerRunning,
    PlayerIdle,
    PlayerJump,
    LosingLife,
    CollectingCoin,
    GameLost,
    GameWon,
    Bg
}

[System.Serializable]
public class Sound
{
    public SoundNames name;
    public AudioClip audioClip;
}

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    Sound[] sounds;
    public AudioSource audioSource;
    public AudioSource bgAudioSource;
    public static SoundManager Instance;

    private void Awake()
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
        GameManager.Instance.gameStateChanged += OnGameStateChanged;
        Player.Instance.playerLivesChanged += OnPlayerLivesChanged;
        Player.Instance.playerCollectedCoin += OnCoinsCollected;
        Player.Instance.playerStateChanged += OnPlayerStateChanged;
    }

    private void OnPlayerStateChanged(PlayerState playerState)
    {
        switch (playerState)
        {
            case PlayerState.Run:
                PlaySFX(SoundNames.PlayerRunning);
                break;
            case PlayerState.Jump:
                PlaySFX(SoundNames.PlayerJump);
                break;
            case PlayerState.Idle:
                PlaySFX(SoundNames.PlayerIdle);
                break;
            default:
                break;
        }
    }

    private void OnCoinsCollected(int obj)
    {
        PlaySFX(SoundNames.CollectingCoin);
    }

    private void OnPlayerLivesChanged(int obj, bool lostLife)
    {
        if (lostLife)
        {
            PlaySFX(SoundNames.LosingLife);
        }
    }

    private void OnGameStateChanged(GameStates gameState)
    {
        switch (gameState)
        {
            case GameStates.GameStart:
                break;
            case GameStates.InGame:
                bgAudioSource.Play();
                break;
            case GameStates.GameEnds:
                bgAudioSource.Stop();
                if (GameManager.Instance.gameWin)
                {
                    PlaySFX(SoundNames.GameWon);
                }
                else
                {
                    PlaySFX(SoundNames.GameLost);
                }
                break;
            default:
                break;
        }
    }

    public void PlaySFX(SoundNames soundName)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == soundName)
            {
                //audioSource.Stop();
                //audioSource.clip = sounds[i].audioClip;
                //audioSource.Play();
                audioSource.PlayOneShot(sounds[i].audioClip);
            }
        }
    }
}
