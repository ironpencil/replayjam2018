using System;
using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    public int playerId;
    [SerializeField]
    private Player rwPlayer;

    public float speed;

    void Awake()
    {
        rwPlayer = ReInput.players.GetPlayer(playerId);
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        float x = rwPlayer.GetAxis("Horizontal");
        float y = rwPlayer.GetAxis("Vertical");

        Debug.Log(String.Format("x: {0}, y: {1}", x, y));
    }
}
