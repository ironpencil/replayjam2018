using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variable/Float")]
public class FloatVariable : ScriptableObject {

    public float Value;

    public bool initValOnEnable = false;
    public float initValue;

    public void OnEnable()
    {
        if (initValOnEnable)
        {
            Value = initValue;
        }
    }
}
