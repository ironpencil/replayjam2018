using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHelper : MonoBehaviour {

    public Transform playerImage;
    public IntVariable playerFacing;
    public MovementConfig movementConfig;
    public Animator playerAnimator;
    public Animator playerAttackAnimator;
    public PlayerAnimConfig animConfig;
    public PlayerState playerState;
    public RuntimeAnimatorController defaultAnimController;
    public RuntimeAnimatorController blackoutAnimController;
    public PlayerData playerData;
    public BlackoutState blackoutState;

    private int previousFacing = 0;

    //private Dictionary<string, bool> boolAnimParams = new Dictionary<string, bool>();

    [SerializeField]
    private PlayerState.State currentState = PlayerState.State.idle;

    private bool resetBlackoutAnimations = false;

    private Rigidbody2D rb;

    // Use this for initialization
    void Start () {
        rb = gameObject.GetComponent<Rigidbody2D>();
        //boolAnimParams[animConfig.isIdleParam] = false;
        //boolAnimParams[animConfig.isRunningParam] = false;
        //boolAnimParams[animConfig.isJumpingParam] = false;
    }

    public void OnStateChanged()
    {
        PlayerState.State newState = playerState.GetState();

        if (newState != currentState)
        {
            SetBoolAnimParam(animConfig.isIdleParam, false);
            SetBoolAnimParam(animConfig.isRunningParam, false);
            SetBoolAnimParam(animConfig.isJumpingParam, false);

            switch (newState)
            {
                case PlayerState.State.idle:
                    SetBoolAnimParam(animConfig.isIdleParam, true);
                    break;
                case PlayerState.State.moving:
                    SetBoolAnimParam(animConfig.isRunningParam, true);
                    break;
                case PlayerState.State.jumping:
                    SetBoolAnimParam(animConfig.isJumpingParam, true);
                    break;
                case PlayerState.State.falling:
                    SetBoolAnimParam(animConfig.isJumpingParam, true);
                    break;
                case PlayerState.State.stunned:
                    playerAnimator.SetTrigger(animConfig.doStunParam);
                    break;
                case PlayerState.State.frozen:
                    playerAttackAnimator.SetBool(animConfig.didHit, true);
                    break;
                default:
                    break;
            }
            currentState = newState;
        }

        //playerAnimator.SetBool(animConfig.isIdleParam, newState == PlayerState.State.idle);
    }

    private void SetBoolAnimParam(string param, bool value)
    {
        //boolAnimParams[param] = value;
        playerAnimator.SetBool(param, value);
    }

    public void OnStartBlackout()
    {
        Debug.Log("Start Blackout");
        if (blackoutState.blackoutPlayer != playerData.playerId)
        {
            resetBlackoutAnimations = true;
            ShowBlackoutAnimations();
        }
    }

    public void OnEndBlackout()
    {
        Debug.Log("End Blackout");
        if (resetBlackoutAnimations)
        {
            ShowDefaultAnimations();
        }
    }

    public void ShowDefaultAnimations()
    {
        playerAnimator.runtimeAnimatorController = defaultAnimController;
    }

    public void ShowBlackoutAnimations()
    {
        Debug.Log("Switch to blackout animations");
        playerAnimator.runtimeAnimatorController = blackoutAnimController;
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