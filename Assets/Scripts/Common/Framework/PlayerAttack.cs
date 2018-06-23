using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject attackObject;
    public AttackConfig attackConfig;
	public AttackBehavior attackBehavior;
    public bool isAttacking;
    public float completeTime;
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
        if (isAttacking && completeTime <= Time.time)
        {
            StopAttack();
        }

        if (!isAttacking && rwPlayer.GetButtonDown("Attack"))
        {
            StartAttack();
        }
    }

    void StartAttack()
    {
		attackObject.SetActive(true);
		Vector2 attackAngle = GetAttackAngle();
		attackBehavior.StartAttack(attackAngle);
		UpdateAttackAngle(attackAngle);
		completeTime = Time.time + attackConfig.duration;
        isAttacking = true;
    }

	void UpdateAttackAngle(Vector2 attackAngle)
	{
		float angle = Mathf.Atan2(attackAngle.y, attackAngle.x) * Mathf.Rad2Deg;
		attackObject.transform.eulerAngles = new Vector3(0, 0, angle);
		//attackObject.transform.RotateAround(transform.position, new Vector3(x, y, 0));
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
        isAttacking = false;
		attackObject.SetActive(false);
		Debug.Log("Hits: " + attackBehavior.hitCount);
    }
}
