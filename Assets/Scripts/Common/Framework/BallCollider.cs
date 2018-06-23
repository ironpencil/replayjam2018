using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCollider : MonoBehaviour
{
	private PlayerBallCollider pbc;
	void Start()
	{
		pbc = GetComponentInParent<PlayerBallCollider>();
	}
    void OnTriggerEnter2D(Collider2D collider)
    {
        BallController bc = collider.GetComponent<BallController>();
        if (bc != null)
        {
            pbc.BallEnter(bc);
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
		BallController bc = collider.GetComponent<BallController>();
        if (bc != null)
        {
            pbc.BallStay(bc);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
		BallController bc = collider.GetComponent<BallController>();
        if (bc != null)
        {
            pbc.BallLeave(bc);
        }
    }
}
