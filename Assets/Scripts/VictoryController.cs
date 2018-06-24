using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryController : MonoBehaviour {

    public Text victoryText;
    public StringVariable victorName;

    private void OnEnable()
    {
        victoryText.text = victorName.Value + " WINS!";
    }
}
