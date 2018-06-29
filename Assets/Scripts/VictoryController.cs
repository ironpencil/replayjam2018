using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryController : MonoBehaviour {

    public Text victoryTextFront;
    public Text victoryTextBack;
    public StringVariable victorName;

    public AudioEvent victorySound;
    public AudioSource audioSource;

    private void OnEnable()
    {
        victorySound.Play(audioSource);
        victoryTextFront.text = victorName.Value.ToLower() + "  wins!";
        victoryTextBack.text = victorName.Value.ToLower() + "  wins!";
    }
}
