using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBallCollider : MonoBehaviour
{
    public GameEvent playerDamagedEvent;
    public PlayerState state;
    public PlayerColorState colorState;

	private PlayerData player;
    private PlayerMovement playerMovement;

	void Start()
	{
		player = GetComponent<PlayerData>();
        playerMovement = GetComponent<PlayerMovement>();
	}

    public void BallEnter(BallController bc)
    {
        if (!state.IsInvulnerable() 
        && player.playerId != colorState.GetBallOwner(bc.color)) {
            player.ChangeHealth(-1);
		    playerDamagedEvent.Raise();
            playerMovement.Stun(bc.GetComponent<Rigidbody2D>().velocity.normalized);
        }
    }

    public void BallStay(BallController bc)
    {
		
    }

    public void BallLeave(BallController bc)
    {
		
    }
}
