using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorUI : MonoBehaviour {
	public List<BallColor> colors;
	public List<GameObject> offUiEls;
	public List<GameObject> onUiEls;

	private Dictionary<BallColor, GameObject> colorOnDict;
	private Dictionary<BallColor, GameObject> colorOffDict;
	public PlayerColorState pcs;

    public int playerNum;

	void Start() {
		colorOnDict = new Dictionary<BallColor, GameObject>();
		colorOffDict = new Dictionary<BallColor, GameObject>();
		int i = 0;
		foreach(BallColor color in colors) {
			colorOnDict.Add(color, onUiEls[i]);
			colorOffDict.Add(color, offUiEls[i++]);
		}
		UpdateColorUI();
	}

	public void UpdateColorUI() {
		DisableAll();

		foreach (BallColor bc in pcs.GetPlayerCollection(playerNum).ballColors)
		{
			colorOnDict[bc].SetActive(true);
			colorOffDict[bc].SetActive(false);
		}
	}

	public void DisableAll() {
		for (int i = 0; i < offUiEls.Count; i++)
		{
			onUiEls[i].SetActive(false);
			offUiEls[i].SetActive(true);
		}
	}
}
