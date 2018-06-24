using System.Collections;
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

	public void OnEnable()
    {
        blackoutPlayer = -1;
    }

	public void CheckForBlackout()
    {
        foreach (PlayerColorCollection player in playerColorState.playerColors) {
            if (player.ballColors.Count >= blackoutCount) {
				Debug.Log("BLACKOUT!!!");
                blackoutPlayer = player.playerNumber;
				blackoutTime = Time.time + blackoutDuration;
				blackoutStartEvent.Raise();
            }
        }
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
			Debug.Log("The blackout has ended...");
			blackoutEndEvent.Raise();
			playerColorState.ClearBallColors(blackoutPlayer);
			blackoutPlayer = -1;
		}
    }

    public override void FixedUpdate()
    {
        
    }
}
