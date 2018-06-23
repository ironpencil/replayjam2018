using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehavior : MonoBehaviour {
	public Vector2 attackAngle;
	public int hitCount = 0;
	public List<Collider2D> hitList;

	void Start()
	{
		hitList = new List<Collider2D>();
	}

	public void StartAttack(Vector2 attackAngle)
	{
		this.attackAngle = attackAngle;
		hitCount = 0;
		hitList = new List<Collider2D>();
	}

	public void StopAttack()
	{

	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		BallController bc = collider.GetComponent<BallController>();
		if (bc != null && !hitList.Contains(collider)) {
			hitList.Add(collider);
			bc.Hit(attackAngle);
			hitCount++;
		}
	}

	void OnTriggerStay2D(Collider2D collider)
	{
		BallController bc = collider.GetComponent<BallController>();
		if (bc != null && !hitList.Contains(collider)) {
			hitList.Add(collider);
			bc.Hit(attackAngle);
			hitCount++;
		}
	}

	void OnTriggerExit2D(Collider2D collider)
	{
		hitList.Remove(collider);
	}
}
