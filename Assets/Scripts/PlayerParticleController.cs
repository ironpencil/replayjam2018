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
    bool isCyanPlaying = false;
    bool isMagentaPlaying = false;
    bool isYellowPlaying = false;

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

    public void EnableColor(BallColor color) 
    {
        SetColorPlaying(color, true);
    }

    public void DisableColor(BallColor color)
    {
        SetColorPlaying(color, false);
    }

    private void SetColorPlaying(BallColor color, bool value)
    {
        if (color == cyan) {
            isCyanPlaying = value;
        } else if (color == magenta) {
            isMagentaPlaying = value;
        } else if (color == yellow) {
            isYellowPlaying = value;
        }
    }

    public void UpdateCyan()
    {
        cyanWrangler.SetPlaying(playerColors.ballColors.Contains(cyan) && isCyanPlaying);
    }

    public void UpdateMagenta()
    {
        magentaWrangler.SetPlaying(playerColors.ballColors.Contains(magenta) && isMagentaPlaying);
    }

    public void UpdateYellow()
    {
        yellowWrangler.SetPlaying(playerColors.ballColors.Contains(yellow) && isYellowPlaying);
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
