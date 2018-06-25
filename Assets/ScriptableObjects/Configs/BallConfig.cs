using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Ball")]
public class BallConfig : ScriptableObject {
    public float wallBounceBleed = 1.0f;
    public float initialSpeed = 10;
    public float mass = 1;
    public float addSpeedOnHit = 1;
    public float maxSpeed = 20;
    public float hitBurst = 20.0f;
    public float minHitSpeed = 6;
}
