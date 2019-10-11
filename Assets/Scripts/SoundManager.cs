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

    public void PlaySFX(SoundNames soundName)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == soundName)
            {
                audioSource.PlayOneShot(sounds[i].audioClip);
            }
        }
    }
}
