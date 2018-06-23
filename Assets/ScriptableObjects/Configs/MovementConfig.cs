using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Movement")]
public class MovementConfig : ScriptableObject {
	public float speed;
	public float initialJumpForce;
    public float holdJumpForce;
    public float maxJumpTime;
    public float jumpDecay;
    public int maxJumps = 2;
}
