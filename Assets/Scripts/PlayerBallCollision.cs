using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBallCollision : MonoBehaviour {

    public IntVariable playerHealth;
    public GameEvent playerDamagedEvent;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;

        BallController ball = other.GetComponent<BallController>();

        if (ball != null)
        {
            playerHealth.Value--;
            playerDamagedEvent.Raise();
        }
    }
}
