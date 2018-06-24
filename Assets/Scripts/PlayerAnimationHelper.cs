using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHelper : MonoBehaviour {

    public Transform playerImage;
    public IntVariable playerFacing;
    public MovementConfig movementConfig;
    public Animator playerAnimator;
    public PlayerAnimConfig animConfig;
    public PlayerState playerState;

    private int previousFacing = 0;

    [SerializeField]
    private PlayerState.State currentState = PlayerState.State.idle;

    private Rigidbody2D rb;

    // Use this for initialization
    void Start () {
        rb = gameObject.GetComponent<Rigidbody2D>();
	}

    public void OnStateChanged()
    {
        PlayerState.State newState = playerState.GetState();

        if (newState != currentState)
        {
            switch (newState)
            {
                case PlayerState.State.idle:
                    Debug.Log("Idle");
                    playerAnimator.SetTrigger(animConfig.doIdleParam);
                    break;
                case PlayerState.State.moving:
                    Debug.Log("Run");
                    playerAnimator.SetTrigger(animConfig.doRunParam);
                    break;
                case PlayerState.State.jumping:
                    Debug.Log("Jump");
                    if (currentState != PlayerState.State.falling)
                    {
                        playerAnimator.SetTrigger(animConfig.doJumpParam);
                    }
                    break;
                case PlayerState.State.falling:
                    break;
                case PlayerState.State.stunned:
                    Debug.Log("Stun");
                    playerAnimator.SetTrigger(animConfig.doStunParam);
                    break;
                default:
                    break;
            }
            currentState = newState;
        }
    }
	
	// Update is called once per frame
	void Update () {
        UpdateImageFacing();
        UpdateRunSpeed();
	}

    private void UpdateImageFacing()
    {
        if (previousFacing != playerFacing.Value)
        {
            Vector3 playerScale = playerImage.localScale;
            playerScale.x = playerFacing.Value;
            playerImage.localScale = playerScale;
            previousFacing = playerFacing.Value;
        }
    }

    private void UpdateRunSpeed()
    {
        float runSpeedMultiplier = Mathf.Abs(rb.velocity.x) / movementConfig.speed;
        runSpeedMultiplier = runSpeedMultiplier * (1.0f - animConfig.minRunMultiplier) + animConfig.minRunMultiplier;
        playerAnimator.SetFloat(animConfig.runMultiplierParam, runSpeedMultiplier);
    }
}