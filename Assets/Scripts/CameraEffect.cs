using UnityEngine;

public class CameraEffect : MonoBehaviour
{
    public Transform cameraResetPosition;
    public Vector2 offset;
    public float followSpeed = 0.5f;
    public float shakeTime = 0.5f;
    public Vector3 shakeForce;
    private Vector2 newCamPosition;
    private Vector2 velocity;

    private void Start()
    {
        GameManager.Instance.gameStateChanged += OnGameStateChanged;
        Player.Instance.playerLivesChanged += OnPlayerLivesChanged;
    }

    private void OnPlayerLivesChanged(int arg1, bool lifeLost)
    {
        if (lifeLost && GameManager.Instance.GameState != GameStates.GameEnds)
        {
            ShakeCamera();
        }
    }

    private void ShakeCamera()
    {
        iTween.ShakePosition(gameObject, shakeForce, shakeTime);
    }

    private void OnGameStateChanged(GameStates gameState)
    {
        switch (gameState)
        {
            case GameStates.GameStart:
                break;
            case GameStates.InGame:
                transform.position = cameraResetPosition.position;
                break;
            case GameStates.GameEnds:
                break;
            default:
                break;
        }
    }

    void LateUpdate()
    {
        newCamPosition = (Vector2)Player.Instance.transform.position + offset;
        transform.position = Vector2.SmoothDamp(transform.position,
            newCamPosition, ref velocity, followSpeed);
    }
}
