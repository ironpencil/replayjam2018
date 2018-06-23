using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour {
	public int playerId;
	public PlayerColorState colorState;

	public void AddBallColor(BallColor color) {
		colorState.AddBallColor(playerId, color);
	}
}
