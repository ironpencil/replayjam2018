using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PVPController : MonoBehaviour {

    public PlayerData playerData;

    public AudioEvent playerHitSound;
    public AudioSource audioSource;

    public void Hit(int playerNum)
    {
        if (playerNum != playerData.playerId)
        {
            //do hit
            playerHitSound.Play(audioSource);
        }
    }
}
