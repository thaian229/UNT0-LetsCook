using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

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
    private float gameplayTimer = 10f;

    private void Awake()
    {
        Instance = this;
        state = State.WaitingToStart;
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
                }
                break;
            case State.CountDownToStart:
                countdownTimer -= Time.deltaTime;
                if (countdownTimer <= 0f)
                {
                    state = State.GamePlaying;
                }
                break;
            case State.GamePlaying:
                gameplayTimer -= Time.deltaTime;
                if (gameplayTimer <= 0f)
                {
                    state = State.GameOver;
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
}
