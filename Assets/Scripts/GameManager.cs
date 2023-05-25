using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event EventHandler OnStateChanged;

    private enum State
    {
        WaitingToStart,
        CountDownToStart,
        GamePlaying,
        GameOver,
    }

    private State state;

    private float waitingTimer = 1f;
    private float countdownTimer = 3f;
    private float gameplayTimerMax = 10f;
    private float gameplayTimer;

    private bool isGamePaused = false;

    private void Awake()
    {
        Instance = this;
        state = State.WaitingToStart;

        gameplayTimer= gameplayTimerMax;
    }

    private void Start()
    {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
    }

    private void GameInput_OnPauseAction(object sender, EventArgs e)
    {
        TogglePauseGame();
    }

    private void Update()
    {
        switch (state)
        {
            case State.WaitingToStart:
                waitingTimer -= Time.deltaTime;
                if (waitingTimer <= 0f)
                {
                    state = State.CountDownToStart;

                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.CountDownToStart:
                countdownTimer -= Time.deltaTime;
                if (countdownTimer <= 0f)
                {
                    state = State.GamePlaying;

                    gameplayTimer = gameplayTimerMax;

                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GamePlaying:
                gameplayTimer -= Time.deltaTime;
                if (gameplayTimer <= 0f)
                {
                    state = State.GameOver;

                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GameOver:
                break;
            default:
                break;
        }
    }

    public bool IsGamePlaying()
    {
        return state == State.GamePlaying;
    }

    public bool IsCountdownToStartActive()
    {
        return state == State.CountDownToStart;
    }

    public bool IsGameOver()
    {
        return state == State.GameOver;
    }

    public float GetCowndownToStartTimer()
    {
        return countdownTimer;
    }

    public float GetGamePlayingTimerNormalized()
    {
        return gameplayTimer / gameplayTimerMax;
    }

    private void TogglePauseGame()
    {
        if (isGamePaused)
        {
            Time.timeScale = 1f;
            isGamePaused = false;
        }
        else
        {
            Time.timeScale = 0f;
            isGamePaused = true;
        }
    }
}
