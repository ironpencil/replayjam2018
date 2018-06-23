using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageCountDisplayer : MonoBehaviour {

    public IntVariable current;
    public IntVariable max;

    public GameObject filledImagePrefab;
    public GameObject emptyImagePrefab;

    [SerializeField]
    private List<GameObject> filledImages = new List<GameObject>();
    [SerializeField]
    private List<GameObject> emptyImages = new List<GameObject>();

    public bool reverseOrder = false;

    void Start()
    {

        if (reverseOrder)
        {
            LoadEmptyImages();
            LoadFilledImages();
        } else
        {
            LoadFilledImages();
            LoadEmptyImages();
        }

        UpdateDisplay();
    }

    private void LoadFilledImages()
    {
        for (int i = 0; i < max.Value; i++)
        {
            filledImages.Add(Instantiate(filledImagePrefab, transform));
        }
    }

    private void LoadEmptyImages()
    {
        for (int i = 0; i < max.Value; i++)
        {
            emptyImages.Add(Instantiate(emptyImagePrefab, transform));
        }
    }

    // Update is called once per frame
    void Update () {
		
	}

    [ContextMenu("Update Display")]
    public void UpdateDisplay()
    {
        int filled = 0;

        foreach (GameObject image in filledImages)
        {
            if (filled < current.Value)
            {
                image.SetActive(true);
                filled++;
            } else
            {
                image.SetActive(false);
            }
        }

        int empty = max.Value - current.Value;

        foreach (GameObject image in emptyImages)
        {
            if (empty > 0)
            {
                image.SetActive(true);
                empty--;
            } else
            {
                image.SetActive(false);
            }
        }
    }

    [ContextMenu("Test Random Value")]
    private void RandomizeCurrentValue()
    {
        current.Value = Random.Range(0, max.Value + 1);
        Debug.Log("ImageCountDisplayer test value: " + current.Value);
        UpdateDisplay();
    }
}
