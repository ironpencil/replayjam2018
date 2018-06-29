using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryController : MonoBehaviour {

    public Text victoryTextFront;
    public Text victoryTextBack;
    public StringVariable victorName;

    private void OnEnable()
    {
        victoryTextFront.text = victorName.Value.ToLower() + "  wins!";
        victoryTextBack.text = victorName.Value.ToLower() + "  wins!";
    }
}
