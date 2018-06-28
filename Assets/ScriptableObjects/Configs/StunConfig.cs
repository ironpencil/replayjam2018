using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Stun")]
public class StunConfig : ScriptableObject {
	public float duration;
	public float strength;
	public float invulnerabilityDuration;
    public float invulnFlashFrequency;
}
