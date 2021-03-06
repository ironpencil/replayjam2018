﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BlackoutState : Updateable {
	public int blackoutCount = 3;
    public int blackoutPlayer = -1;
	public float blackoutDuration;
	private float blackoutTime;
    public GameEvent blackoutStartEvent;
	public GameEvent blackoutEndEvent;
	public PlayerColorState playerColorState;
    public GameManager gameState;

	public void OnEnable()
    {
        blackoutPlayer = -1;
    }

	public void CheckForBlackout()
    {
        if (gameState.CurrentState == GameManager.GameState.RoundActive)
        {
            foreach (PlayerColorCollection player in playerColorState.playerColors)
            {
                if (player.ballColors.Count >= blackoutCount)
                {
                    blackoutPlayer = player.playerNumber;
                    blackoutTime = Time.time + blackoutDuration;
                    blackoutStartEvent.Raise();
                }
            }
        }
    }

    public void EndBlackout()
    {
        if (blackoutPlayer > -1)
        {
            playerColorState.ClearBallColors(blackoutPlayer);
        }
        blackoutPlayer = -1;
        blackoutEndEvent.Raise();
    }

	public bool InBlackout()
    {
        if (blackoutPlayer > -1)
        {
            return true;
        }
        return false;
    }

    public override void Start()
    {
        
    }

    public override void Update()
    {
        if (Time.time > blackoutTime && InBlackout())
		{
            EndBlackout();
		}
    }

    public override void FixedUpdate()
    {
        
    }
}
