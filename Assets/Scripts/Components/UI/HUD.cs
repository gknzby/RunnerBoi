using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RunnerBoi;

namespace RunnerBoi.Component
{
    public class HUD : MonoBehaviour
    {
        [Header("Position Tracker")]
        [SerializeField] private GameObject position_Obj;
        [SerializeField] private GameObject positionText_Obj;

        [Header("Wallpaint")]
        [SerializeField] private GameObject wallpaint_Obj;
        [SerializeField] private GameObject wallpaintPerText_Obj;
        [SerializeField] private GameObject nextLevelBtn_Obj;

        private int runnerCount = 1;

        private void Start()
        {
            Actions.Instance.OnGameStateChange += HandleGameStateChange;
        }

        private void HandlePositionChange(int position)
        {
            string posTxt = position.ToString() + "/" + runnerCount.ToString();
            positionText_Obj.GetComponent<UnityEngine.UI.Text>().text = posTxt;
        }

        private void HandlePaintPerChange(int percentage)
        {
            string perTxt = percentage.ToString() + "%";
            wallpaintPerText_Obj.GetComponent<UnityEngine.UI.Text>().text = perTxt;
            if (percentage > 70 && !nextLevelBtn_Obj.activeSelf)
            {
                nextLevelBtn_Obj.SetActive(true);
            }
        }

        private void HandleGameStateChange(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.OnReady:
                    GameOnReady();
                    break;
                case GameState.RaceFinished:
                    GameRaceFinished();                    
                    break;
                case GameState.Wallpaint:
                    GameWallpaint();                    
                    break;
                default:
                    break;
            }
        }

        private void GameOnReady()
        {
            if (Managers.LevelManager.Instance.Data.CurrentGameMode == GameMode.PVE)
            {
                Actions.Instance.OnPositionChange += HandlePositionChange;
                position_Obj.SetActive(true);
                runnerCount = Managers.LevelManager.Instance.Data.RunnerCount;
            }
            Actions.Instance.OnPaintPercantageChange -= HandlePaintPerChange;
            wallpaint_Obj.SetActive(false);
        }

        private void GameRaceFinished()
        {
            if (Managers.LevelManager.Instance.Data.CurrentGameMode == GameMode.PVE)
            {
                Actions.Instance.OnPositionChange -= HandlePositionChange;
                position_Obj.SetActive(false);
            }
        }

        private void GameWallpaint()
        {
            Actions.Instance.OnPaintPercantageChange += HandlePaintPerChange;
            wallpaint_Obj.SetActive(true);
            nextLevelBtn_Obj.SetActive(false);
        }

    }
}

