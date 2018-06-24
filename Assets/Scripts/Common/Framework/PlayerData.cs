using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour {
	public int playerId;
	public IntVariable playerHealth;
	public PlayerColorState colorState;
    public GameEvent playerDeathEvent;

    private bool isDead = false;

	public void AddBallColor(BallColor color) {
		colorState.AddBallColor(playerId, color);
	}

	public void ChangeHealth(int amount) {
		playerHealth.Value += amount;
	}

    public void Update()
    {
        if (!isDead)
        {
            if (playerHealth.Value <= 0)
            {
                isDead = true;
                playerDeathEvent.Raise();
                gameObject.SetActive(false);
                Destroy(gameObject);
            }
        }
    }
}
