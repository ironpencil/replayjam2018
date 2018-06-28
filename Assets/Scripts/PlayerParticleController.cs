using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticleController : MonoBehaviour {

    public ParticleSystemWrangler cyanWrangler;
    public ParticleSystemWrangler magentaWrangler;
    public ParticleSystemWrangler yellowWrangler;

    public PlayerData playerData;

    public BallColor cyan;
    public BallColor magenta;
    public BallColor yellow;

    public bool checkColorsOnUpdate = false;

    [SerializeField]
    private PlayerColorCollection playerColors;

	// Use this for initialization
	void Start () {
        playerColors = playerData.colorState.GetPlayerCollection(playerData.playerId);
	}
	
	// Update is called once per frame
	void Update () {
		if (checkColorsOnUpdate)
        {
            UpdateAllPlayerParticles();
        }
	}

    [ContextMenu("Update All Particles")]
    public void UpdateAllPlayerParticles()
    {
        UpdateCyan();
        UpdateMagenta();
        UpdateYellow();
    }

    public void UpdateCyan()
    {
        cyanWrangler.SetPlaying(playerColors.ballColors.Contains(cyan));
    }

    public void UpdateMagenta()
    {
        magentaWrangler.SetPlaying(playerColors.ballColors.Contains(magenta));
    }

    public void UpdateYellow()
    {
        yellowWrangler.SetPlaying(playerColors.ballColors.Contains(yellow));
    }

    public void UpdateColorParticles(BallColor color)
    {
        if (color == cyan)
        {
            UpdateCyan();
        }
        else if (color == magenta)
        {
            UpdateMagenta();
        }
        else if (color == yellow)
        {
            UpdateYellow();
        }
    }
}
