using UnityEngine;
using System;

public enum ObjectsTags
{
    Platform,
    Coin,
    Spike
}
public class SpawnedObject : MonoBehaviour
{
    public int spawnLimit = -1;
    public bool respawn = true;
    public float spawnRate;
    public float startTime;
    [HideInInspector]
    public Vector2 spawnPosition;
    [HideInInspector]
    public Vector2 referencePosition;
    public ObjectsTags SPAWNED_OBJECT_TAG;

    private void Awake()
    {
        gameObject.tag = SPAWNED_OBJECT_TAG.ToString();
        GameManager.Instance.gameStateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged(GameStates gameState)
    {
        switch (gameState)
        {
            case GameStates.GameStart:
                break;
            case GameStates.InGame:
                break;
            case GameStates.GameEnds:
                Destroy(this.gameObject);
                break;
            default:
                break;
        }
    }
}
