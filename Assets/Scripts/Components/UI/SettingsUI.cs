using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RunnerBoi.Component
{
    public class SettingsUI : MonoBehaviour
    {
        [Header("Game Mode")]
        [SerializeField] private GameObject SingleBtn_Obj;
        [SerializeField] private GameObject PVEBtn_Obj;

        [ColorUsage(true)]
        [SerializeField] private Color SelectedGameModeColor;
        [ColorUsage(true)]
        [SerializeField] private Color NormalGameModeColor;

        private GameMode gameMode = GameMode.Single;
        

        [Header("Runner Count")]
        [SerializeField] private GameObject RunnerCountText_Obj;
        [SerializeField] private GameObject RCIncBtn_Obj;
        [SerializeField] private GameObject RCDecBtn_Obj;

        private int runnerCount = 1;

        private void Awake()
        {
            SingleBtn_Obj.GetComponent<Image>().color = SelectedGameModeColor;
            PVEBtn_Obj.GetComponent<Image>().color = NormalGameModeColor;

            RunnerCountText_Obj.GetComponent<Text>().text = runnerCount.ToString();
        }

        public void RunnerCountChange(bool isIncrement)
        {
            if (gameMode == GameMode.Single)
                return;

            if(isIncrement)
            {
                if (9 < runnerCount)
                    return;
                else
                    runnerCount++;
            }
            else
            {
                if (runnerCount < 3)
                    return;
                else
                    runnerCount--;
            }

            RunnerCountText_Obj.GetComponent<Text>().text = runnerCount.ToString();
        }

        public void SelectSingleplayer()
        {
            if (gameMode == GameMode.Single)
                return;

            gameMode = GameMode.Single;
            runnerCount = 1;

            SingleBtn_Obj.GetComponent<Image>().color = SelectedGameModeColor;
            PVEBtn_Obj.GetComponent<Image>().color = NormalGameModeColor;

            RunnerCountText_Obj.GetComponent<Text>().text = runnerCount.ToString();
        }

        public void SelectPVE()
        {
            if (gameMode == GameMode.PVE)
                return;

            gameMode = GameMode.PVE;
            runnerCount = 2;

            SingleBtn_Obj.GetComponent<Image>().color = NormalGameModeColor;
            PVEBtn_Obj.GetComponent<Image>().color = SelectedGameModeColor;

            RunnerCountText_Obj.GetComponent<Text>().text = runnerCount.ToString();
        }

        public void UpdateGameSettings()
        {
            Managers.LevelManager.Instance.UpdateLevelSettings(gameMode, runnerCount);
        }

    }
}
