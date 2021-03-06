﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameManager", menuName = "GameManager", order = 1)]
public class GameManager : ScriptableObject {

    public enum GameState
    {
        Title,
        RoundActive,
        Victory,
        Pause
    }
    
    [SerializeField]
    private GameState initialState;

    [SerializeField]
    private GameState currentState;

    public GameState CurrentState { get { return currentState; } }

    public GameEvent startRoundEvent;

    public bool acceptGameInput = true;

    public void StartGame()
    {
        SetState(GameState.RoundActive);
        startRoundEvent.Raise();
    }

    public void SetState(GameState nextState)
    {
        currentState = nextState;
    }

    public void OnEnable()
    {
        SetState(initialState);
    }
}
