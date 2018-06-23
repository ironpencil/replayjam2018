using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Ball")]
public class BallConfig : ScriptableObject {

    public float initialSpeed = 10;
    public float mass = 1;
    public float addSpeedOnHit = 1;
    public float maxSpeed = 20;
}
