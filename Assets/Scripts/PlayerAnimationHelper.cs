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
            playerAnimator.SetBool(animConfig.isIdleParam, false);
            playerAnimator.SetBool(animConfig.isRunningParam, false);
            playerAnimator.SetBool(animConfig.isJumpingParam, false);

            switch (newState)
            {
                case PlayerState.State.idle:
                    playerAnimator.SetBool(animConfig.isIdleParam, true);
                    break;
                case PlayerState.State.moving:
                    playerAnimator.SetBool(animConfig.isRunningParam, true);
                    break;
                case PlayerState.State.jumping:
                    playerAnimator.SetBool(animConfig.isJumpingParam, true);
                    break;
                case PlayerState.State.falling:
                    playerAnimator.SetBool(animConfig.isJumpingParam, true);
                    break;
                case PlayerState.State.stunned:
                    playerAnimator.SetTrigger(animConfig.doStunParam);
                    break;
                case PlayerState.State.frozen:
                    // TODO not sure how to stop the animation
                    break;
                default:
                    break;
            }
            currentState = newState;
        }

        //playerAnimator.SetBool(animConfig.isIdleParam, newState == PlayerState.State.idle);
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