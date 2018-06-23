using System;
using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    public int playerId;
    [SerializeField]
    private Player rwPlayer;

    private Rigidbody2D rb;
    public float speed;
    public Transform[] groundChecks;
    public float initialJumpForce;
    public float holdJumpForce;
    public float maxJumpTime;
    public float jumpDecay;
    private int maxJumps = 2;

    public PhysicsMaterial2D noFrictionMaterial;

    private int jumpCount = 0;
    private bool beginJump = false;
    private bool jumping = false;
    private float jumpTime = 0.0f;
    private float currentJumpForce = 0.0f;
    private bool grounded = false;
    private PhysicsMaterial2D originalMaterial;
    private Collider2D col;

    private enum State
    {
        moving,
        still,
        jumping
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
    }

    void Awake()
    {
        rwPlayer = ReInput.players.GetPlayer(playerId);
    }

    void Update()
    {
        grounded = false;

        foreach (Transform groundCheck in groundChecks)
        {
            // Sean commented this out... maybe if we don't want the ground to be on another layer, we don't
            // need to do the layermask stuff? Unsure... testing. 
            // 
            // DO NOT REMOVE THE COMMENTED OUT LINE OF CODE
            //
            if (Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground")))
            {
                grounded = true;
                break;
            }
        }

        // If the jump button is pressed and the player is grounded then the player should jump.
        if (rwPlayer.GetButtonDown("Jump") && grounded)
        {
            beginJump = true;
        }

        if (grounded && !beginJump && !jumping)
        {
            //we're on the ground, we're not jumping, make sure our collision material is correct
            if (col.sharedMaterial != originalMaterial)
            {
                col.sharedMaterial = originalMaterial;

                // seems to be a bug that currently requires the collider to be reset to refresh material
                col.enabled = !col.enabled;
                col.enabled = !col.enabled;
            }
        }
    }

    void FixedUpdate()
    {
        float directionY = rwPlayer.GetAxis("Vertical");
        float directionX = rwPlayer.GetAxis("Horizontal");

        float desiredVelocityX = 0.0f;

        desiredVelocityX = speed * directionX;

        float velocityX = Mathf.Lerp(rb.velocity.x, desiredVelocityX, Time.fixedDeltaTime * 10);
        float velocityY = rb.velocity.y;

        if (beginJump)
        {
            jumpTime = Time.time;
            jumping = true;
            currentJumpForce = holdJumpForce;

            velocityY += initialJumpForce;

            col.sharedMaterial = noFrictionMaterial;

            // seems to be a bug that currently requires the collider to be reset to refresh material
            col.enabled = !col.enabled;
            col.enabled = !col.enabled;
        }
        else if (jumping)
        {
            float jumpDuration = Time.time - jumpTime;

            if (CanAddJumpForce(jumpDuration) && currentJumpForce > 0.0f)
            {
                velocityY += currentJumpForce;
                currentJumpForce = currentJumpForce - (Time.fixedDeltaTime * rb.gravityScale * rb.mass * jumpDecay);
            }
            else
            {
                jumping = false;
                currentJumpForce = 0.0f;
            }
        }

        beginJump = false;

        rb.velocity = new Vector2(velocityX, velocityY);

        //Debug.Log("Velocity = " + rb.velocity.ToString());
    }

    private bool CanAddJumpForce(float jumpDuration)
    {
        //if they stopped pressing Jump, stop jumping
        if (!rwPlayer.GetButton("Jump"))
        {
            return false;
        }

        //if they are at max air time, stop jumping
        if (jumpDuration > maxJumpTime)
        {
            return false;
        }

        //if their jump was stopped (due to colliding with something)
        if ((rb.velocity.y <= 0) && !beginJump)
        {
            return false;
        }

        return true;
    }

    // Leaving this here just to see what i was doing before
    // this can be removed after a commit
    // void Move()
    // {
    //     float x = 0;

    //     if (rb.velocity.x == 0 && rb.velocity.y == 0)
    //     {
    //         state = State.still;
    //     }
    //     else if (rb.velocity.y != 0)
    //     {
    //         state = State.jumping;
    //     }
    //     else
    //     {
    //         state = State.moving;
    //     }

    //     x = rwPlayer.GetAxis("Horizontal") * speed;

    //     if (state != State.jumping)
    //     {
    //         if (rwPlayer.GetButtonDown("Jump"))
    //         {
    //             // These vars were public floats ealier
    //             float horizontalJumpBoost = 0;
    //             float jump = 0;
    //             ///////////////////

    //             float hjump = horizontalJumpBoost * rwPlayer.GetAxis("Horizontal");
    //             rb.AddForce(new Vector2(hjump, jump), ForceMode2D.Impulse);
    //             state = State.jumping;
    //         }
    //     }

    //     //Debug.Log(String.Format("x: {0}, y: {1}", x, y));
    //     rb.AddForce(new Vector2(x, 0));
    // }
}
