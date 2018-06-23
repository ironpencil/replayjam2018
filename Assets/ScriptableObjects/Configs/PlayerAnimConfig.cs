using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Player Animation")]
public class PlayerAnimConfig : ScriptableObject {

    public float minRunMultiplier = 0.3f;
    public string runMultiplierParam = "runMultiplier";
}
