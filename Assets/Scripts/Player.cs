using System;
using System.Collections;
using UnityEngine;

public enum PlayerState
{
    Run,
    Jump,
    Idle
}

public class Player : MonoBehaviour
{
    public float accelerationRate = 4;
    public float maxXVelocity = 6;
    public Transform playerFeet;
    public float jumpPower;
    public float overlapRadius;
    public Transform resetTransform;
    public LayerMask groundLayer;
    public Animator coinAnimator;
    public Animator livesAnimator;
    public ParticleSystem dust;
    public Action<int> playerCollectedCoin;
    public Action<int, bool> playerLivesChanged;
    public Action<PlayerState> playerStateChanged;
    [SerializeField]
    private PlayerState playerState;
    public PlayerState PlayerState
    {
        set
        {
            playerState = value;
            playerStateChanged?.Invoke(playerState);
        }
        get
        {
            return playerState;
        }
    }
    [SerializeField]
    private int livesCount;
    public int LivesCount
    {
        set
        {
            if (value < livesCount)
            {
                livesAnimator.SetTrigger("LifeLost");
                livesCount = value;
                playerLivesChanged?.Invoke(livesCount, true);
            }
            else
            {
                livesCount = value;
                playerLivesChanged?.Invoke(livesCount, false);
            }
        }
        get
        {
            return livesCount;
        }
    }
    [SerializeField]
    private int coinsCollected;
    public int CoinsCollected
    {
        set
        {
            coinsCollected = value;
            coinAnimator.SetTrigger("CollectCoin");
            playerCollectedCoin?.Invoke(coinsCollected);
        }
        get
        {
            return coinsCollected;
        }
    }
    private Rigidbody2D body2d;
    public static Player Instance;
    private Vector3 velocity = Vector2.zero;
    private Animator animator;
    private bool doOnce;
    private Vector3 targetVelocity;
    private float lerpTime;

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

    void Start()
    {
        animator = GetComponent<Animator>();
        InputManager.Instance.jumpPressed += Jump;
        InputManager.Instance.runPressed += Run;
        GameManager.Instance.gameStateChanged += OnGameStateChanged;
        LivesCount = GameManager.Instance.maxLives;
        body2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (body2d.velocity.y <= 0.3f && body2d.velocity.y >= -0.05f)
        {
            if (!doOnce)
            {
                dust.Play();
                doOnce = true;
                Invoke("StopParticles", 0.3f);
            }
        }
        if (body2d.velocity == Vector2.zero)
        {
            PlayerState = PlayerState.Idle;
        }
    }

    private void Run(bool pressed)
    {
        animator.SetTrigger("Run");
        targetVelocity = new Vector2(maxXVelocity, body2d.velocity.y);
        lerpTime += Time.fixedDeltaTime * accelerationRate;
        body2d.velocity = Vector3.Lerp(body2d.velocity, targetVelocity, lerpTime);
        if (!pressed)
        {
            lerpTime = 0;
        }
    }

    private void Jump()
    {
        // to avoid a double jump, check first of the player is on ground.
        // multiply the jump power with player height
        if (IsOnGround())
        {
            body2d.AddForce(new Vector2(0, jumpPower * transform.localScale.y), ForceMode2D.Impulse);
            animator.SetTrigger("Jump");
            doOnce = false;
        }
    }

    private void OnGameStateChanged(GameStates gameState)
    {
        switch (gameState)
        {
            case GameStates.GameStart:
                break;
            case GameStates.InGame:
                Player.Instance.CoinsCollected = 0;
                body2d.velocity = Vector2.zero;
                ResetPosition();
                break;
            case GameStates.GameEnds:
                if (!GameManager.Instance.gameWin)
                {
                    // reset lives and coins
                    LivesCount = GameManager.Instance.maxLives;
                }
                break;
            default:
                break;
        }
    }

    private void ResetPosition()
    {
        transform.position = resetTransform.position;
    }

    private bool IsOnGround()
    {
        var overlappingColliders = Physics2D.OverlapCircle(playerFeet.position,
            overlapRadius, groundLayer);
        if (overlappingColliders != null)
        {
            return true;
        }
        return false;
    }

    private void StopParticles()
    {
        dust.Stop();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(playerFeet.position, overlapRadius);
    }
}
