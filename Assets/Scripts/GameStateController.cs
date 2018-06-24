using Rewired;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateController : MonoBehaviour {

    public GameManager gameManager;

    public GameEvent showTitleEvent;
    public GameEvent prepareRoundEvent;

    public GameEvent roundStartEvent;
    public GameEvent roundEndEvent;

    public GameEvent pauseEvent;
    public GameEvent unpauseEvent;

    public PlayerState playerOneState;
    public PlayerState playerTwoState;

    public IntVariable playerOneHealth;
    public IntVariable playerOneMaxHealth;

    public IntVariable playerTwoHealth;
    public IntVariable playerTwoMaxHealth;

    public IntVariable playerOneFacing;
    public IntVariable playerTwoFacing;

    public Transform playerOneStartPos;
    public Transform playerTwoStartPos;

    public GameObject playerOnePrefab;
    public GameObject playerTwoPrefab;

    public StringVariable gameVictor;

    public PlayerColorState playerColorManager;

    [SerializeField]
    private GameObject playerOneInstance;
    [SerializeField]
    private GameObject playerTwoInstance;

    public List<Transform> ballStartPositions;
    public List<GameObject> ballPrefabs;

    [SerializeField]
    private List<GameObject> ballInstances;

    private Player menuPlayer;

    // Use this for initialization
    void Start () {
        OnPause();
        ClearRound();
        gameManager.SetState(GameManager.GameState.Title);
        PrepareRound();
        showTitleEvent.Raise();
	}
	
	// Update is called once per frame
	void Update () {
        switch (gameManager.CurrentState)
        {
            case GameManager.GameState.Title:
                HandleTitleState();
                break;
            case GameManager.GameState.RoundActive:
                HandleRoundActiveState();
                break;
            case GameManager.GameState.Victory:
                HandleVictoryState();
                break;
            case GameManager.GameState.Pause:
                HandlePauseState();
                break;
            default:
                break;
        }
    }

    public void PlayerOneWins()
    {
        playerOneState.SetInvulnerable(true);
        DisplayVictory("SKULL");
    }

    public void PlayerTwoWins()
    {
        playerTwoState.SetInvulnerable(true);
        DisplayVictory("RAZE");
    }

    public void DisplayVictory(string victor)
    {
        gameVictor.Value = victor;
        gameManager.SetState(GameManager.GameState.Victory);
        roundEndEvent.Raise();

        StartCoroutine(DisplayTitleAfterSeconds(GameManager.GameState.Victory, 2.0f));
    }

    private IEnumerator DisplayTitleAfterSeconds(GameManager.GameState fromState, float numSeconds)
    {
        yield return new WaitForSecondsRealtime(numSeconds);

        if (gameManager.CurrentState == fromState)
        {
            OnPause();
            ClearRound();
            PrepareRound();
            gameManager.SetState(GameManager.GameState.Title);
        }
    }

    private void OnPause()
    {
        gameManager.acceptGameInput = false;
        Time.timeScale = 0;
    }

    private void OnUnpause()
    {
        gameManager.acceptGameInput = true;
        Time.timeScale = 1.0f;
    }

    private void HandlePauseState()
    {
        if (menuPlayer.GetButtonDown("Start"))
        {
            gameManager.SetState(GameManager.GameState.RoundActive);
            OnUnpause();
            unpauseEvent.Raise();
        }
    }

    private void HandleVictoryState()
    {
        for (int i = 0; i < ReInput.players.playerCount; i++)
        {
            Player player = ReInput.players.GetPlayer(i);
            if (player.GetButtonDown("Start"))
            {
                OnPause();
                ClearRound();
                PrepareRound();
                gameManager.SetState(GameManager.GameState.Title);
            }
        }
    }


    private void HandleRoundActiveState()
    {
        for (int i = 0; i < ReInput.players.playerCount; i++)
        {
            Player player = ReInput.players.GetPlayer(i);
            if (player.GetButtonDown("Start"))
            {
                gameManager.SetState(GameManager.GameState.Pause);
                pauseEvent.Raise();
                OnPause();
                menuPlayer = player;
            }
        }
    }

    private void HandleTitleState()
    {
        for (int i = 0; i < ReInput.players.playerCount; i++)
        {
            Player player = ReInput.players.GetPlayer(i);
            if (player.GetButtonDown("Start"))
            {
                gameManager.SetState(GameManager.GameState.RoundActive);
                OnUnpause();
                roundStartEvent.Raise();
            }
        }
    }

    [ContextMenu("Reset Round")]
    public void ResetRound()
    {
        ClearRound();
        PrepareRound();
    }

    public void ClearRound()
    {
        if (playerOneInstance != null)
        {
            GameObject.DestroyImmediate(playerOneInstance);
        }

        if (playerTwoInstance != null)
        {
            GameObject.DestroyImmediate(playerTwoInstance);
        }

        //prepare balls
        foreach (GameObject ball in ballInstances)
        {
            if (ball != null)
            {
                DestroyImmediate(ball);
            }
        }

        ballInstances.Clear();

        playerColorManager.ClearBallColors(0);
        playerColorManager.ClearBallColors(1);
    }

    public void PrepareRound()
    {
        OnPause();

        playerOneHealth.Value = playerOneMaxHealth.Value;
        playerTwoHealth.Value = playerTwoMaxHealth.Value;

        playerOneFacing.Value = playerOneFacing.initValue;
        playerTwoFacing.Value = playerTwoFacing.initValue;

        playerOneState.SetInvulnerable(false);
        playerTwoState.SetInvulnerable(false);

        playerColorManager.InitializePlayer(0);
        playerColorManager.InitializePlayer(1);

        playerOneInstance = Instantiate(playerOnePrefab, playerOneStartPos.position, Quaternion.identity, transform.parent);
        playerTwoInstance = Instantiate(playerTwoPrefab, playerTwoStartPos.position, Quaternion.identity, transform.parent);

        for (int i = 0; i < ballPrefabs.Count; i++)
        {
            GameObject ballPrefab = ballPrefabs[i];
            Vector3 startPos = ballStartPositions[i].position;
            GameObject ballInstance = Instantiate(ballPrefab, startPos, Quaternion.identity, transform.parent);
            ballInstances.Add(ballInstance);
        }

        prepareRoundEvent.Raise();
    }
}
