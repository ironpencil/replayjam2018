using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableObjectUpdater : MonoBehaviour {
	public List<Updateable> updateables;

	// Use this for initialization
	void Start () {
		foreach(Updateable updateable in updateables) {
			updateable.Start();
		}
	}
	
	// Update is called once per frame
	void Update () {
		foreach(Updateable updateable in updateables) {
			updateable.Update();
		}
	}

	void FixedUpdate() {
		foreach(Updateable updateable in updateables) {
			updateable.FixedUpdate();
		}
	}
}
