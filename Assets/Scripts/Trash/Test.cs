using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

    public GameManager gameManager;

    public GameEvent startGameEvent;

	// Use this for initialization
	void Awake () {
        Debug.Log(gameManager.CurrentState);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            startGameEvent.Raise();
        }
	}
}
