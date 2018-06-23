using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {

    public BallConfig ballConfig;

    public float currentSpeed = 0.0f;

    private Rigidbody2D rb;
	// Use this for initialization
	void Awake () {
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.mass = ballConfig.mass;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space))
        {
            HitRandomly();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Stop();
        }
	}

    public void Hit(Vector2 direction)
    {
        rb.velocity = Vector2.zero;

        if (currentSpeed == 0.0f)
        {
            currentSpeed = ballConfig.initialSpeed;
        } else
        {
            currentSpeed = Mathf.Min(ballConfig.maxSpeed, currentSpeed + ballConfig.addSpeedOnHit);
        }

        rb.AddForce(direction.normalized * currentSpeed, ForceMode2D.Impulse);
    }

    [ContextMenu("Hit Randomly")]
    public void HitRandomly()
    {
        Vector2 direction = new Vector2();

        direction.x = Random.Range(-1.0f, 1.0f);
        direction.y = Random.Range(-1.0f, 1.0f);

        Hit(direction);
    }

    [ContextMenu("Stop")]
    public void Stop()
    {
        rb.velocity = Vector2.zero;
        currentSpeed = 0.0f;
    }
}
