using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorUI : MonoBehaviour {
	public List<BallColor> colors;
	public List<GameObject> uiEls;

	private Dictionary<BallColor, GameObject> colorDict;
	public PlayerColorState pcs;

	public PlayerData player;

	void Start() {
		colorDict = new Dictionary<BallColor, GameObject>();
		int i = 0;
		foreach(BallColor color in colors) {
			colorDict.Add(color, uiEls[i++]);
		}
		UpdateColorUI();
	}

	public void UpdateColorUI() {
		DisableAll();

		foreach (BallColor bc in pcs.GetPlayerCollection(player.playerId).ballColors)
		{
			colorDict[bc].SetActive(true);
		}
	}

	public void DisableAll() {
		foreach(GameObject el in uiEls) {
			el.SetActive(false);
		}
	}
}
