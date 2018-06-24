using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variable/Int")]
public class IntVariable : ScriptableObject {

    public int Value;

    public bool initValOnEnable = false;
    public int initValue;

    public void OnEnable()
    {
        if (initValOnEnable)
        {
            Value = initValue;
        }
    }


}
