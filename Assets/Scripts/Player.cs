using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform playerFeet;
    public float runSpeed;
    public float jumpPower;
    public float overlapRadius;
    public LayerMask groundLayer;
    public Action playerHitSpike;
    [SerializeField]
    private int livesCount;
    public int LivesCount
    {
        set
        {
            if (value < LivesCount)
            {
                playerHitSpike?.Invoke();
            }
            livesCount = value;
        }
        get
        {
            return livesCount;
        }
    }
    private Rigidbody2D body2d;

    void Start()
    {
        InputManager.Instance.jumpPressed += Jump;
        InputManager.Instance.runPressed += Run;
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

    public void OnHitBySpike()
    {
        LivesCount--;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(playerFeet.position, overlapRadius);
    }
}
