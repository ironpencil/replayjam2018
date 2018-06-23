using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBallCollider : MonoBehaviour
{
    public GameEvent playerDamagedEvent;
	private PlayerData player;

	void Start()
	{
		player = GetComponent<PlayerData>();
	}

    public void BallEnter(BallController bc)
    {
		player.ChangeHealth(-1);
		playerDamagedEvent.Raise();
    }

    public void BallStay(BallController bc)
    {
		
    }

    public void BallLeave(BallController bc)
    {
		
    }
}
