﻿using System;
using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Player rwPlayer;
    private PlayerData player;

    private Rigidbody2D rigidBody;
    public Transform[] groundChecks;
    public MovementConfig movementConfigs;

    public PhysicsMaterial2D noFrictionMaterial;

    private float stunTime;
    private float invulnTime;
    public StunConfig stun;
    public bool isInvulnerable;

    public int facing = 1;
    private int jumpCount = 0;
    private bool beginJump = false;
    private bool jumping = false;
    private float jumpTime = 0.0f;
    private float currentJumpForce = 0.0f;
    private bool grounded = false;
    private PhysicsMaterial2D originalMaterial;
    private Collider2D col;
    
    public State state;
    public enum State
    {
        moving,
        still,
        jumping,
        falling,
        stunned
    }

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        player = GetComponent<PlayerData>();
        rwPlayer = ReInput.players.GetPlayer(player.playerId);
        state = State.still;
    }

    void Update()
    {
        grounded = false;

        foreach (Transform groundCheck in groundChecks)
        {
            if (Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground")))
            {
                grounded = true;
                jumpCount = 0;
                break;
            }
        }

        // If the jump button is pressed and the player is grounded then the player should jump.
        if (rwPlayer.GetButtonDown("Jump") && jumpCount < movementConfigs.maxJumps)
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
        if (isInvulnerable && Time.time > invulnTime)
        {
            isInvulnerable = false;
            GetComponentInChildren<SpriteRenderer>().color = Color.white;
        }
        if (state == State.stunned)
        {
            if (Time.time > stunTime)
            {
                UnStun();
            }
        } else {
            HandleMovementInput();
        }
    }

    public void Stun(Vector2 direction)
    {
        stunTime = Time.time + stun.duration;
        invulnTime = Time.time + stun.invulnerabilityDuration;
        isInvulnerable = true;
        state = State.stunned;
        float velocityX = rigidBody.velocity.x;
        float velocityY = rigidBody.velocity.y;
        
        if (rigidBody.velocity.y > 0) {
            velocityY = 0;
        }
        if (rigidBody.velocity.x > 0 && direction.x < 0) {
            velocityX = 0;
        } else if (rigidBody.velocity.x < 0 && direction.x > 0) {
            velocityX = 0;
        }
        rigidBody.velocity = new Vector2(velocityX, velocityY);
        rigidBody.AddForce(direction * stun.strength, ForceMode2D.Impulse);
        GetComponentInChildren<SpriteRenderer>().color = Color.red;
    }

    public void UnStun() {
        state = State.still;
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
            state = State.jumping;
            jumpTime = Time.time;
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
            float jumpDuration = Time.time - jumpTime;

            if (CanAddJumpForce(jumpDuration) && currentJumpForce > 0.0f)
            {
                velocityY += currentJumpForce;
                currentJumpForce = currentJumpForce - (Time.fixedDeltaTime * rigidBody.gravityScale * rigidBody.mass * movementConfigs.jumpDecay);
            }
            else
            {
                jumping = false;
                currentJumpForce = 0.0f;
                state = State.falling;
            }
        }

        beginJump = false;

        if (velocityX < 0) facing = -1;
        if (velocityX > 0) facing = 1;

        if (grounded) {
            if (velocityX == 0) {
                state = State.still;
            } else {
                state = State.moving;
            }
        }

        rigidBody.velocity = new Vector2(velocityX, velocityY);
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

        if (state == State.stunned)
        {
            return false;
        }

        return true;
    }
}
