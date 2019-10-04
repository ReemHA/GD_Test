using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform playerFeet;
    public float runSpeed;
    public float jumpPower;
    public float overlapRadius;
    public Transform resetTransform;
    public LayerMask groundLayer;
    public Action<int> playerCollectedCoin;
    public Action<int> playerLivesChanged;
    [SerializeField]
    private int livesCount;
    public int LivesCount
    {
        set
        {
            livesCount = value;
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
            playerCollectedCoin?.Invoke(coinsCollected);
        }
        get
        {
            return coinsCollected;
        }
    }
    private Rigidbody2D body2d;
    public static Player Instance;

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
        InputManager.Instance.jumpPressed += Jump;
        InputManager.Instance.runPressed += Run;
        GameManager.Instance.gameStateChanged += OnGameStateChanged;
        LivesCount = 3;
        body2d = GetComponent<Rigidbody2D>();
    }

    private void Run(bool run)
    {
        Vector3 targetVelocity;
        Vector3 velocity = Vector2.zero;
        if (run)
        {
            targetVelocity = new Vector2(runSpeed, body2d.velocity.y);
            body2d.velocity = Vector3.SmoothDamp(body2d.velocity, targetVelocity, ref velocity, Time.fixedDeltaTime);
        }
        else
        {
            targetVelocity = new Vector2(0, body2d.velocity.y);
            body2d.velocity = targetVelocity;
        }

    }

    private void Jump()
    {
        // to avoid a double jump, check first of the player is on ground.
        if (IsOnGround())
        {
            body2d.AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);
        }
    }

    private void OnGameStateChanged(GameStates gameState)
    {
        switch (gameState)
        {
            case GameStates.GameStart:
                break;
            case GameStates.InGame:
                LivesCount = 3;
                CoinsCollected = 0;
                body2d.velocity = Vector2.zero;
                ResetPosition();
                break;
            case GameStates.GameEnds:
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
