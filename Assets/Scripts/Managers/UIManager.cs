using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RunnerBoi;


namespace RunnerBoi.Managers
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }
        
        [Header("Menus")]
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private GameObject tapToStart;
        [SerializeField] private GameObject finishMenu;
        [SerializeField] private GameObject lostMenu;
        [SerializeField] private GameObject endGameMenu;

        private void Awake()
        {
            Instance = this;

            this.mainMenu.SetActive(true);
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
                    this.tapToStart.SetActive(true);
                    break;
                case GameState.RaceLost:
                    this.lostMenu.SetActive(true);
                    break;
                case GameState.Endgame:
                    this.endGameMenu.SetActive(true);
                    break;
                default:
                    break;
            }
        }

        public void HandleUICommand(UICommand uiCmd)
        {
            switch (uiCmd)
            {
                case UICommand.StartGame:
                    GameManager.Instance.StartGame();
                    break;
                case UICommand.StartRace:
                    GameManager.Instance.StartRace();
                    break;
                case UICommand.NextLevel:
                    GameManager.Instance.NextLevel();
                    break;
                case UICommand.ResetLevel:
                    GameManager.Instance.ResetLevel();
                    break;
                case UICommand.RestartGame:
                    GameManager.Instance.RestartGame();
                    break;
                default:
                    break;
            }
        }

        public void HandleUIComponent(RunnerBoi.Component.UIComponent uiComponent)
        {
            HandleUICommand(uiComponent.uiCmd);
        }
    }
}

