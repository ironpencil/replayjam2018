﻿using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject attackObject;
    public AttackConfig attackConfig;
	public AttackBehavior attackBehavior;
    public BlackoutState blackoutState;
    public PlayerState state;
    public float completeTime;
    public float recoveryTime;
    private Player rwPlayer;
    private PlayerData player;
    public IntVariable playerFacing;

    // Use this for initialization
    void Start()
    {
        player = GetComponent<PlayerData>();
        rwPlayer = ReInput.players.GetPlayer(player.playerId);
		attackObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if ((state.IsAttacking() && completeTime <= Time.time) 
        || state.IsStunned())
        {
            StopAttack();
        }

        if(!state.IsAttacking()
        && !state.IsStunned()
        && rwPlayer.GetButtonDown("Attack") 
        && Time.time > recoveryTime)
        {
            // If we're not in blackout, or you are the blackout
            // player, then attack. If you're not the blackout
            // player, may Heaven help you
            if (!blackoutState.InBlackout() 
            || blackoutState.blackoutPlayer == player.playerId) {
                StartAttack();
            }
        }
    }

    void StartAttack()
    {
		attackObject.SetActive(true);
		Vector2 attackAngle = GetAttackAngle();
		attackBehavior.StartAttack(attackAngle);
		UpdateAttackAngle(attackAngle);
		completeTime = Time.time + attackConfig.duration;
        recoveryTime = Time.time + attackConfig.duration + attackConfig.recovery;
        state.StartAttack();
    }

	void UpdateAttackAngle(Vector2 attackAngle)
	{
		float angle = Mathf.Atan2(attackAngle.y, attackAngle.x) * Mathf.Rad2Deg;

        Vector3 attackScale = attackObject.transform.localScale;

        if (angle < -90 || angle > 90)
        {
            angle += 180;
            attackScale.x = -1;
        } else
        {
            attackScale.x = 1;
        }

        attackObject.transform.localScale = attackScale;
		attackObject.transform.eulerAngles = new Vector3(0, 0, angle);
	}

	Vector2 GetAttackAngle()
	{
		float x = rwPlayer.GetAxis("Horizontal");
		float y = rwPlayer.GetAxis("Vertical");
		if (x == 0f) {
			x = playerFacing.Value;
		}
		return new Vector2(x, y);
	}

    void StopAttack()
    {
		attackObject.SetActive(false);
        state.StopAttack();
		//Debug.Log("Hits: " + attackBehavior.hitCount);
    }
}
