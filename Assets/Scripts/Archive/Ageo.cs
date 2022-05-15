using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RunnerBoi;

public class Ageo : MonoBehaviour
{
    private NavMeshAgent nma;

    private void Awake()
    {
        nma = GetComponent<NavMeshAgent>();

        nma.isStopped = true;
    }

    private void Start()
    {
        Actions.Instance.OnGameStateChange += HandleGameStateChange;
    }

    private void HandleGameStateChange(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.OnReady:
                break;
            case GameState.Racing:
                nma.isStopped = false;
                break;
            case GameState.RaceFinished:
                nma.isStopped = true;
                break;
            case GameState.Wallpaint:
                break;
            default:
                break;
        }
    }
}
