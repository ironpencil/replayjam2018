using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {
    public BallConfig ballConfig;
    public BallColor color;
    public float currentSpeed = 0.0f;
    [SerializeField]
    private float minimumSpeed;
    public AudioEvent ballHitWall;
    public AudioSource ballAudioSource;

    private Rigidbody2D rb;
	// Use this for initialization
	void Awake () {
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.mass = ballConfig.mass;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        MaintainVelocity();
    }

    public void MaintainVelocity()
    {
        Vector2 vel = rb.velocity;

        if (vel.sqrMagnitude != currentSpeed*currentSpeed)
        {
            rb.velocity = vel.normalized * currentSpeed;
        }
    }

    public void Hit(int playerId, Vector2 direction)
    {
        rb.velocity = Vector2.zero;

        if (currentSpeed == 0.0f)
        {
            minimumSpeed = ballConfig.initialSpeed;
        } else
        {
            minimumSpeed = Mathf.Min(ballConfig.maxSpeed, currentSpeed + ballConfig.addSpeedOnHit);
        }

        currentSpeed = minimumSpeed + ballConfig.hitBurst;
        if (currentSpeed < ballConfig.minHitSpeed) {
            currentSpeed = ballConfig.minHitSpeed;
        }
        rb.velocity = direction.normalized * currentSpeed;
        //rb.AddForce(direction.normalized * currentSpeed, ForceMode2D.Impulse);
    }

    public void OnCollisionEnter2D(Collision2D collision) {
        currentSpeed = currentSpeed - ballConfig.wallBounceBleed;
        if (currentSpeed < minimumSpeed) {
            currentSpeed = minimumSpeed;
        }
        PlayAudioEvent(ballHitWall, ballAudioSource);
    }

    [ContextMenu("Hit Randomly")]
    public void HitRandomly()
    {
        Vector2 direction = new Vector2();

        direction.x = Random.Range(-1.0f, 1.0f);
        direction.y = Random.Range(-1.0f, 1.0f);

        Hit(0, direction);
    }

    [ContextMenu("Stop")]
    public void Stop()
    {
        rb.velocity = Vector2.zero;
        currentSpeed = 0.0f;
    }

    private void PlayAudioEvent(AudioEvent audioE, AudioSource audioS) {
        if (audioE != null && audioS != null) {
            audioE.Play(audioS);
        }
    }
}
