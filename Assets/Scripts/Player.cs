using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int maxLives = 3;
    public int maxCoins = 10;
    public float maxRunSpeed = 4;
    public Transform playerFeet;
    public float jumpPower;
    public float overlapRadius;
    public Transform resetTransform;
    public LayerMask groundLayer;
    public Animator coinAnimator;
    public Animator livesAnimator;
    public Action<int> playerCollectedCoin;
    public Action<int> playerLivesChanged;
    [SerializeField]
    private int livesCount;
    public int LivesCount
    {
        set
        {
            livesCount = value;
            livesAnimator.SetTrigger("LifeLost");
            playerLivesChanged?.Invoke(livesCount);
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
    private float runSpeed = 4;
    private Vector3 velocity = Vector2.zero;
    private Animator animator;

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
        LivesCount = maxLives;
        body2d = GetComponent<Rigidbody2D>();
    }

    private void Run()
    {
        animator.SetTrigger("Run");
        Vector3 targetVelocity;
        targetVelocity = new Vector2(runSpeed, body2d.velocity.y);
        body2d.velocity = Vector3.SmoothDamp(body2d.velocity, targetVelocity, ref velocity, Time.fixedDeltaTime * maxRunSpeed);
        //runSpeed = Mathf.Lerp(runSpeed, maxRunSpeed, Time.fixedDeltaTime * );
    }

    private void Jump()
    {
        // to avoid a double jump, check first of the player is on ground.
        // multiply the jump power with player height
        if (IsOnGround())
        {
            body2d.AddForce(new Vector2(0, jumpPower * transform.localScale.y), ForceMode2D.Impulse);
            animator.SetTrigger("Jump");
        }
    }

    private void OnGameStateChanged(GameStates gameState)
    {
        switch (gameState)
        {
            case GameStates.GameStart:
                break;
            case GameStates.InGame:
                body2d.velocity = Vector2.zero;
                ResetPosition();
                break;
            case GameStates.GameEnds:
                if (!GameManager.Instance.gameWin)
                {
                    // reset lives and coins
                    LivesCount = maxLives;
                   // CoinsCollected = 0;
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(playerFeet.position, overlapRadius);
    }
}
