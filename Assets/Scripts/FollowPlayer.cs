using System;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform cameraResetPosition;
    public Vector2 offset;
    private Vector2 newCamPosition;
    private float lerpTime = 0.5f;

    private void Start()
    {
        GameManager.Instance.gameStateChanged += OnGameStateChanged;
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
        lerpTime += Time.fixedDeltaTime;
        transform.position = Vector2.MoveTowards(transform.position, newCamPosition, lerpTime);
    }
}
