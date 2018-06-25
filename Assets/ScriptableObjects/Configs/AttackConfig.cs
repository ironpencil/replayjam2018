using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Attack")]
public class AttackConfig : ScriptableObject {
	public float duration;
    public float strength;
    public float recovery;
    public float hitFreezeLength;
}
