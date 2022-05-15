using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RunnerBoi;
using System;
using UnityEngine.AI;

namespace RunnerBoi.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            Time.timeScale = 0;
        }

        public void StartGame()
        {
            LevelManager.Instance.NextLevel();

            Actions.Instance.OnGameStateChange?.Invoke(GameState.OnReady);
            Time.timeScale = 1;
        }

        public void EndGame()
        {
            Actions.Instance.OnGameStateChange?.Invoke(GameState.Endgame);
        }

        public void StartRace()
        {
            if (LevelManager.Instance.Data.CurrentGameMode == GameMode.PVE)
            {
                this.gameObject.AddComponent<RunnerBoi.Workers.PositionTracker>();
            }

            Actions.Instance.OnGameStateChange?.Invoke(GameState.Racing);
        }

        public void RaceLost()
        {
            FinishRace();
            Actions.Instance.OnGameStateChange?.Invoke(GameState.RaceLost);
        }

        public void RaceWon()
        {
            FinishRace();
            Actions.Instance.OnGameStateChange?.Invoke(GameState.RaceWon);
        }

        public void PaintWall()
        {
            Actions.Instance.OnGameStateChange?.Invoke(GameState.Wallpaint);
        }

        private void FinishRace()
        {
            if (LevelManager.Instance.Data.CurrentGameMode == GameMode.PVE)
            {
                Destroy(this.gameObject.GetComponent<Workers.PositionTracker>());
            }

            Actions.Instance.OnGameStateChange?.Invoke(GameState.RaceFinished);

            ////Temp to paint
            //Actions.Instance.OnGameStateChange?.Invoke(GameState.Wallpaint);
            //PaintStage();
        }

        public void ResetLevel()
        {
            if(LevelManager.Instance.RetryLevel())
                Actions.Instance.OnGameStateChange?.Invoke(GameState.OnReady);
        }

        public void NextLevel()
        {
            if(LevelManager.Instance.NextLevel())
                Actions.Instance.OnGameStateChange?.Invoke(GameState.OnReady);
            
        }

        public void RestartGame()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }

    }
}

