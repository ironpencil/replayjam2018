using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public StunConfig stun;

    public GameManager gameState;

    private int jumpCount = 0;
    private bool beginJump = false;
    private bool jumping = false;
    private float startJumpTime = 0.0f;
    private float currentJumpForce = 0.0f;
    private bool wasGrounded = true;
    private bool isGrounded = true;
    private PhysicsMaterial2D originalMaterial;
    private Collider2D collider;

    private PlayerState.State recoverState = PlayerState.State.idle;

    public AudioSource movementAudioSource;
    public AudioEvent jumpAudioEvent;
    public AudioEvent landAudioEvent;
    public AudioEvent runAudioEvent;
    public AudioEvent stunAudioEvent;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        player = GetComponent<PlayerData>();
        rwPlayer = ReInput.players.GetPlayer(player.playerId);
        state.SetState(PlayerState.State.idle);
        originalMaterial = collider.sharedMaterial;
    }

    void Update()
    {
        if (!state.IsFrozen()) {
            isGrounded = IsGrounded();

            if (isGrounded && !wasGrounded)
            {
                PlayAudioEvent(landAudioEvent, movementAudioSource);
            }

            wasGrounded = isGrounded;

            if (isGrounded) {
                jumpCount = 0;
            }
            
            // If the jump button is pressed and the player is grounded then the player should jump.
            if (gameState.acceptGameInput && rwPlayer.GetButtonDown("Jump")
            && jumpCount < movementConfigs.maxJumps
            && !state.IsStunned())
            {
                beginJump = true;
                jumpCount++;
            }

            if (isGrounded && !beginJump && !jumping)
            {
                //we're on the ground, we're not jumping, make sure our collision material is correct
                if (collider.sharedMaterial != originalMaterial)
                {
                    collider.sharedMaterial = originalMaterial;

                    // seems to be a bug that currently requires the collider to be reset to refresh material
                    collider.enabled = !collider.enabled;
                    collider.enabled = !collider.enabled;
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (!state.IsFrozen()) {
            if (state.IsInvulnerable() && Time.time > state.invulnerableTime)
            {
                state.SetInvulnerable(false);
                //GetComponentInChildren<SpriteRenderer>().color = Color.white;
            }
            if (state.IsStunned())
            {
                if (Time.time > state.stunTime)
                {
                    UnStun();
                }
            }
            else
            {
                HandleMovementInput();
            }
        } else {
            CheckUnfrozen();
        }
    }

    public void CheckUnfrozen()
    {
        if (state.frozenTime < Time.time) {
            state.SetState(recoverState);
            rigidBody.gravityScale = state.gravityScaleHold;
            rigidBody.velocity = state.velocityHold;
        }
    }

    public void Freeze(float frozenTime)
    {
        if (state.GetState() != PlayerState.State.frozen)
        {
            state.frozenTime = Time.time + frozenTime;
            recoverState = state.GetState();
            state.SetState(PlayerState.State.frozen);
            state.velocityHold = rigidBody.velocity;
            rigidBody.velocity = Vector2.zero;
            state.gravityScaleHold = rigidBody.gravityScale;
            rigidBody.gravityScale = 0;
            // Need to increase timers by the freeze time
            // so they continue as if nothing happened
            startJumpTime += frozenTime;
            state.invulnerableTime += frozenTime;

            PlayerAttack pa = GetComponent<PlayerAttack>();
            if (pa != null)
            {
                pa.Freeze(frozenTime);
            }
        }
    }

    public void UnStun()
    {
        state.SetState(PlayerState.State.idle);
        //GetComponentInChildren<SpriteRenderer>().color = Color.blue;
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

            collider.sharedMaterial = noFrictionMaterial;

            // seems to be a bug that currently requires the collider to be reset to refresh material
            collider.enabled = !collider.enabled;
            collider.enabled = !collider.enabled;

            PlayAudioEvent(jumpAudioEvent, movementAudioSource);
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

        if (isGrounded && !jumping && !state.IsStunned())
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
        state.stunTime = Time.time + stun.duration;
        state.invulnerableTime = Time.time + stun.invulnerabilityDuration;
        jumping = false;
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
        StartCoroutine(FlashSpriteForDuration(stun.invulnerabilityDuration, stun.invulnFlashFrequency));
        PlayAudioEvent(stunAudioEvent, movementAudioSource);
    }

    private IEnumerator FlashSpriteForDuration(float duration, float frequency)
    {
        List<SpriteRenderer> sprites = GetComponentsInChildren<SpriteRenderer>(true).ToList();

        Color flashColor = Color.clear;

        float minWaitTime = 0.01f;
        float startTime = Time.time;
        float endTime = startTime + duration;

        while (Time.time < endTime)
        {
            foreach (var sprite in sprites)
            {
                sprite.color = flashColor;
            }

            float waitTime = Mathf.Max(minWaitTime, frequency * Mathf.InverseLerp(endTime, startTime, Time.time));
            yield return new WaitForSeconds(frequency);

            for (int i = 0; i < sprites.Count; i++)
            {
                sprites[i].color = Color.white;
            }

            waitTime = Mathf.Max(minWaitTime, frequency * Mathf.InverseLerp(endTime, startTime, Time.time));
            yield return new WaitForSeconds(frequency);
        }
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

    private void PlayAudioEvent(AudioEvent audioE, AudioSource audioS) {
        if (audioE != null && audioS != null) {
            audioE.Play(audioS);
        }
    }
}
