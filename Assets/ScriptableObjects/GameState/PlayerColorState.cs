using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class PlayerColorState : ScriptableObject {

    [SerializeField]
    List<PlayerColorCollection> playerColors;

    [SerializeField]
    int TEST_ONLY_PLAYER;

    [SerializeField]
    BallColor TEST_ONLY_BALL;

    public void OnEnable()
    {
        playerColors = new List<PlayerColorCollection>();
    }

    public PlayerColorCollection InitializePlayer(int playerNum)
    {
        PlayerColorCollection player = GetPlayerCollection(playerNum);

        if (player == null)
        {
            player = new PlayerColorCollection() { playerNumber = playerNum };
            playerColors.Add(player);
        }

        player.ballColors.Clear();

        return player;
    }

    public void AddBallColor(int playerNum, BallColor ballColor)
    {
        PlayerColorCollection player = GetPlayerCollection(playerNum);

        if (player == null)
        {
            Debug.LogError("Error adding ballColor '" + ballColor.name + "' to player " + playerNum + ": player not found.");
            return;
        }

        if (!player.ballColors.Contains(ballColor))
        {
            player.ballColors.Add(ballColor);
        }
    }

    public void RemoveBallColor(int playerNum, BallColor ballColor)
    {
        PlayerColorCollection player = GetPlayerCollection(playerNum);

        if (player == null)
        {
            Debug.LogError("Error removing ballColor '" + ballColor.name + "' from player " + playerNum + ": player not found.");
            return;
        }

        if (player.ballColors.Contains(ballColor))
        {
            player.ballColors.Remove(ballColor);
        }
    }

    public void ClearBallColors(int playerNum)
    {
        PlayerColorCollection player = GetPlayerCollection(playerNum);

        if (player == null)
        {
            Debug.LogError("Error clearing ball colors for player " + playerNum + ": player not found.");
            return;
        }

        player.ballColors.Clear();
    }

    public PlayerColorCollection GetPlayerCollection(int playerNum)
    {
        PlayerColorCollection player = playerColors.FirstOrDefault(p => { return p.playerNumber == playerNum; });

        return player;
    }

    [ContextMenu("Test Initialize Two Players")]
    private void TestInitializePlayers()
    {
        InitializePlayer(0);
        InitializePlayer(1);
    }

    [ContextMenu("Test Add Ball")]
    private void TestAddBall()
    {
        AddBallColor(TEST_ONLY_PLAYER, TEST_ONLY_BALL);
    }

    [ContextMenu("Test Remove Ball")]
    private void TestRemoveBall()
    {
        RemoveBallColor(TEST_ONLY_PLAYER, TEST_ONLY_BALL);
    }

    [ContextMenu("Test Clear Balls")]
    private void TestClearBalls()
    {
        ClearBallColors(TEST_ONLY_PLAYER);
    }
}
