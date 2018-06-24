using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHelper : MonoBehaviour {

    public Transform playerImage;
    public IntVariable playerFacing;
    public MovementConfig movementConfig;
    public Animator playerAnimator;
    public PlayerAnimConfig animConfig;

    private int previousFacing = 0;

    private Rigidbody2D rb;
    

    // Use this for initialization
    void Start () {
        rb = gameObject.GetComponent<Rigidbody2D>();
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