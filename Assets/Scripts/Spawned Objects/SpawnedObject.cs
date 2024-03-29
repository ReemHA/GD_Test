﻿using UnityEngine;
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
    public float yOffset = -8.6f;
    public float xOffset;
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
                GameManager.Instance.gameStateChanged += OnGameStateChanged;
                break;
            case GameStates.GameEnds:
                GameManager.Instance.gameStateChanged -= OnGameStateChanged;
                Destroy(this.gameObject);
                break;
            default:
                break;
        }
    }
}
