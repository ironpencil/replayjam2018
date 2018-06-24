using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variable/String")]
public class StringVariable : ScriptableObject {

    public string Value;

    public bool initValOnEnable = false;
    public string initValue;

    public void OnEnable()
    {
        if (initValOnEnable)
        {
            Value = initValue;
        }
    }
}
