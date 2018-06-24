using System;
using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Player rwPlayer;
    private PlayerData player;
    public PlayerState state;

    private Rigidbody2D rigidBody;
    public Transform[] groundChecks;
    public MovementConfig movementConfigs;
    public PhysicsMaterial2D noFrictionMaterial;

    public IntVariable facing;
    private float stunTime;
    private float invulnTime;
    public StunConfig stun;

    private int jumpCount = 0;
    private bool beginJump = false;
    private bool jumping = false;
    private float startJumpTime = 0.0f;
    private float currentJumpForce = 0.0f;
    private bool grounded = false;
    private PhysicsMaterial2D originalMaterial;
    private Collider2D col;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        player = GetComponent<PlayerData>();
        rwPlayer = ReInput.players.GetPlayer(player.playerId);
        state.SetState(PlayerState.State.idle);
    }

    void Update()
    {
        grounded = IsGrounded();

        if (grounded) {
            jumpCount = 0;
        }
        
        // If the jump button is pressed and the player is grounded then the player should jump.
        if (rwPlayer.GetButtonDown("Jump")
        && jumpCount < movementConfigs.maxJumps
        && !state.IsStunned())
        {
            beginJump = true;
            jumpCount++;
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
        if (state.IsInvulnerable() && Time.time > invulnTime)
        {
            state.SetInvulnerable(false);
            GetComponentInChildren<SpriteRenderer>().color = Color.white;
        }
        if (state.IsStunned())
        {
            if (Time.time > stunTime)
            {
                UnStun();
            }
        }
        else
        {
            HandleMovementInput();
        }
    }


    public void UnStun()
    {
        state.SetState(PlayerState.State.idle);
        GetComponentInChildren<SpriteRenderer>().color = Color.blue;
    }

    private void HandleMovementInput()
    {
        float directionX = rwPlayer.GetAxis("Horizontal");
        float desiredVelocityX = movementConfigs.speed * directionX;

        float velocityX = Mathf.Lerp(rigidBody.velocity.x, desiredVelocityX, Time.fixedDeltaTime * 10);
        float velocityY = rigidBody.velocity.y;

        if (beginJump)
        {
            state.SetState(PlayerState.State.jumping);
            startJumpTime = Time.time;
            jumping = true;
            currentJumpForce = movementConfigs.holdJumpForce;

            // Wanted double jump to feel like we're jumping off of the ground again.
            //velocityY += initialJumpForce;
            velocityY = movementConfigs.initialJumpForce;

            col.sharedMaterial = noFrictionMaterial;

            // seems to be a bug that currently requires the collider to be reset to refresh material
            col.enabled = !col.enabled;
            col.enabled = !col.enabled;
        }
        else if (jumping)
        {
            float jumpDuration = Time.time - startJumpTime;

            if (CanAddJumpForce(jumpDuration) && currentJumpForce > 0.0f)
            {
                velocityY += currentJumpForce;
                currentJumpForce = currentJumpForce - (Time.fixedDeltaTime * rigidBody.gravityScale * rigidBody.mass * movementConfigs.jumpDecay);
            }
            else
            {
                jumping = false;
                currentJumpForce = 0.0f;
                state.SetState(PlayerState.State.falling);
            }
        }

        beginJump = false;

        if (velocityX < 0) facing.Value = -1;
        if (velocityX > 0) facing.Value = 1;

        if (grounded && !jumping && !state.IsStunned())
        {
            if (velocityX == 0)
            {
                state.SetState(PlayerState.State.idle);
            }
            else
            {
                state.SetState(PlayerState.State.moving);
            }
        }

        rigidBody.velocity = new Vector2(velocityX, velocityY);
    }

    bool IsGrounded()
    {
        foreach (Transform groundCheck in groundChecks)
        {
            if (Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground")))
            {
                return true;
            }
        }
        return false;
    }
    
    public void Stun(Vector2 direction)
    {
        stunTime = Time.time + stun.duration;
        invulnTime = Time.time + stun.invulnerabilityDuration;
        state.SetInvulnerable(true);
        state.SetState(PlayerState.State.stunned);
        float velocityX = rigidBody.velocity.x;
        float velocityY = rigidBody.velocity.y;

        if (rigidBody.velocity.y > 0)
        {
            velocityY = 0;
        }
        if (rigidBody.velocity.x > 0 && direction.x < 0)
        {
            velocityX = 0;
        }
        else if (rigidBody.velocity.x < 0 && direction.x > 0)
        {
            velocityX = 0;
        }
        rigidBody.velocity = new Vector2(velocityX, velocityY);
        rigidBody.AddForce(direction * stun.strength, ForceMode2D.Impulse);
        GetComponentInChildren<SpriteRenderer>().color = Color.red;
    }
    private bool CanAddJumpForce(float jumpDuration)
    {
        //if they stopped pressing Jump, stop jumping
        if (!rwPlayer.GetButton("Jump"))
        {
            return false;
        }

        //if they are at max air time, stop jumping
        if (jumpDuration > movementConfigs.maxJumpTime)
        {
            return false;
        }

        //if their jump was stopped (due to colliding with something)
        if ((rigidBody.velocity.y <= 0) && !beginJump)
        {
            return false;
        }

        //if they are stunned while jumping, stop jumping
        if (state.IsStunned())
        {
            return false;
        }

        return true;
    }
}
