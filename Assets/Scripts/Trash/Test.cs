using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

    public GameManager gameManager;

	// Use this for initialization
	void Awake () {
        Debug.Log(gameManager.CurrentState);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
