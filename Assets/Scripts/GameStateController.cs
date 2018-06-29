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

    public BlackoutState blackoutState;

    [SerializeField]
    private GameObject playerOneInstance;
    [SerializeField]
    private GameObject playerTwoInstance;

    public List<Transform> ballStartPositions;
    public List<GameObject> ballPrefabs;
    public List<GameObject> jumperPrefabs;

    public FloatVariable timeScale;

    [SerializeField]
    private List<GameObject> ballInstances = new List<GameObject>();

    private Player menuPlayer;

    public AudioEvent confirmUIEvent;
    public AudioEvent cancelUIEvent;

    public AudioSource uiAudioSource;

    private List<GameObject> jumperInstances = new List<GameObject>();

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
        DisplayVictory("SKULLKID");
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
        timeScale.Value = 0;
    }

    private void OnUnpause()
    {
        gameManager.acceptGameInput = true;
        // Hack, if you unpause during something that
        // affects timescale 
        Time.timeScale = 1;
        timeScale.Value = 1;
    }

    private void HandlePauseState()
    {
        if (menuPlayer.GetButtonDown("Start"))
        {
            cancelUIEvent.Play(uiAudioSource);
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
                confirmUIEvent.Play(uiAudioSource);
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
                confirmUIEvent.Play(uiAudioSource);
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
                confirmUIEvent.Play(uiAudioSource);
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

        //prepare jumpers
        foreach (GameObject jumper in jumperInstances)
        {
            if (jumper != null)
            {
                DestroyImmediate(jumper);
            }
        }

        jumperInstances.Clear();

        playerColorManager.ClearBallColors(0);
        playerColorManager.ClearBallColors(1);

        blackoutState.EndBlackout();
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

            GameObject jumperInstance = Instantiate(jumperPrefabs[i], ballInstance.transform);
            Jumper jumper = jumperInstance.GetComponent<Jumper>();
            //jumper.followColor = ballInstance.GetComponent<BallController>().color;
            //jumper.colorState = playerColorManager;
            jumper.players.Add(playerOneInstance.transform);
            jumper.players.Add(playerTwoInstance.transform);
            jumper.ball = ballInstance.transform;
        }

        prepareRoundEvent.Raise();
    }
}
