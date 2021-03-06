﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerState/PlayerState")]
public class PlayerState : ScriptableObject {
	public GameEvent playerStateChangedEvent;

	[SerializeField]
	private bool isAttacking = false;
	[SerializeField]
	private bool isInvulnerable = false;

	public float invulnerableTime;
	public float frozenTime;
	public float stunTime;

	public float gravityScaleHold;
	public Vector2 velocityHold;

	public enum State {
		idle,
		moving,
		jumping,
		falling,
		stunned,
		frozen
	}

	public void OnEnable()
    {
        isAttacking = false;
		isInvulnerable = false;
    }

	public bool IsIdle() {
		return this.value == State.idle;
	}

	public bool IsMoving() {
		return this.value == State.moving;
	}

	public bool IsJumping() {
		return this.value == State.jumping;
	}

	public bool IsFalling() {
		return this.value == State.falling;
	}

	public bool IsStunned() {
		return this.value == State.stunned;
	}

	public bool IsAttacking() {
		return isAttacking;
	}

	public bool IsInvulnerable() {
		return isInvulnerable;
	}

	public bool IsFrozen() {
		return this.value == State.frozen;
	}

	public void SetState(State nState) {
        if (this.value != nState)
        {
            this.value = nState;
            playerStateChangedEvent.Raise();
        }
	}

	public void StartAttack() {
		isAttacking = true;
		playerStateChangedEvent.Raise();
	}

	public void StopAttack() {
		isAttacking = false;
		playerStateChangedEvent.Raise();
	}

	public void SetInvulnerable(bool invuln) {
		this.isInvulnerable = invuln;
		playerStateChangedEvent.Raise();
	}

	public State GetState() {
		return value;
	}

	[SerializeField]
	private State value;

    [ContextMenu("Make Invulnerable")]
    private void MakeInvulnerable()
    {
        invulnerableTime = float.MaxValue;
        isInvulnerable = true;
    }
}
