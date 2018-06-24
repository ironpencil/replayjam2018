using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Player Animation")]
public class PlayerAnimConfig : ScriptableObject {

    public string doRunParam = "doRun";
    public string doIdleParam = "doIdle";
    public string doJumpParam = "doJump";
    public string doFallParam = "doFall";
    public string doStunParam = "doStun";

    public float minRunMultiplier = 0.3f;
    public string runMultiplierParam = "runMultiplier";
}
