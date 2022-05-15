using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RunnerBoi;
using RunnerBoi.Character;

namespace RunnerBoi.Managers
{
    public class LevelManager : MonoBehaviour
    {
        #region Data Classes

        [System.Serializable]
        public class LevelData
        {
            public GameObject Level_Obj;
            public Transform FinishPos_T;
        }

        [System.Serializable]
        public class RaceData
        {
            public Transform Player_T;
            public Transform[] StartPos_T;
            [HideInInspector] public GameMode CurrentGameMode;
            [HideInInspector] public List<Transform> Runners_T;
            [HideInInspector] public int RunnerCount;
            [HideInInspector] public int PlayerPlace;
        }

        #endregion

        public static LevelManager Instance { get; private set; }        

        [Header("Required Data")]
        [SerializeField]
        [Tooltip("Player Transform (From Scene)")]
        private Transform Player_T;

        [SerializeField]
        [Tooltip("Parent Transform of Runners")] 
        private Transform RunnersParent_T;

        [SerializeField]
        [Tooltip("Opponent Runner Prefab (From Assets)")] 
        private GameObject OpponentRunner_Prefab;

        [SerializeField]
        private GameObject FinishArea_Obj;

        [SerializeField]
        private Transform[] StartPos_T;

        [Header("Levels")]
        public List<LevelData> Levels;

        [HideInInspector] public RaceData Data;
        [HideInInspector] public LevelData CurrentLevel;

        private int currentLevelIndex;

        private void Awake()
        {
            Instance = this;

            this.Data.Player_T = Player_T;
            currentLevelIndex = -1;
            this.Data.StartPos_T = this.StartPos_T;
        }

        private void Start()
        {
            for(int i = 0; i < Levels.Count; i++)
            {
                Levels[i].Level_Obj.SetActive(false);
            }

            Actions.Instance.OnPositionChange += HandlePositionChange;
        }


        public void UpdateLevelSettings(GameMode gameMode, int runnerCount)
        {
            this.Data.CurrentGameMode = gameMode;
            this.Data.RunnerCount = runnerCount;
        }

        private bool LoadLevel(int index)
        {
            if (index < 0)
            {
                Debug.LogError("Unexpected level index input");
                return false;
            }
            if(this.Levels.Count <= index)
            {
                GameManager.Instance.EndGame();
                return false;
            }

            //IDK
            currentLevelIndex = index;
            //Load level stuffs
            this.CurrentLevel = this.Levels[index];
            this.CurrentLevel.Level_Obj.SetActive(true);
            this.SpawnRunners();
            this.FinishArea_Obj.transform.position = this.CurrentLevel.FinishPos_T.position + new Vector3(0,0,18);

            return true;
        }

        private void HandlePositionChange(int position)
        {
            this.Data.PlayerPlace = position;
        }

        public bool RetryLevel()
        {
            int index = this.Levels.IndexOf(CurrentLevel);
            return this.LoadLevel(index);
        }

        public bool NextLevel()
        {
            if(currentLevelIndex < 0)
            {
                this.LoadLevel(0);
                return false;
            }

            this.CurrentLevel.Level_Obj.SetActive(false);
            int index = this.Levels.IndexOf(CurrentLevel);
            return this.LoadLevel(index + 1);

        }

        private void SpawnRunners()
        {
            if(this.Data.RunnerCount < this.Data.Runners_T.Count) //If needed runner count less than current count, 
            {
                int i;
                for (i = 0; i < this.Data.RunnerCount; i++) //First, reposition the old runners
                {
                    this.Data.Runners_T[i].GetComponent<Runner>().NewRace(this.Data.StartPos_T[i], this.CurrentLevel.FinishPos_T);
                }

                for(; i < this.Data.Runners_T.Count; i++) //Then, clear remaining runners
                {
                    this.Data.Runners_T[i].GetComponent<Runner>().DestroySelf();
                    this.Data.Runners_T.RemoveAt(i);
                }
            }
            else
            {
                if(this.Data.Runners_T.Count == 0) //First Initialize Player
                {
                    this.Data.Runners_T.Add(this.Data.Player_T);                    
                }
                this.Data.Runners_T[0].GetComponent<Runner>().NewRace(this.Data.StartPos_T[0], this.CurrentLevel.FinishPos_T);

                int i = 1;
                for(; i < this.Data.Runners_T.Count; i++) //Reposition old runners
                {
                    this.Data.Runners_T[i].GetComponent<Runner>().NewRace(this.Data.StartPos_T[i], this.CurrentLevel.FinishPos_T);
                }
                for(; i < this.Data.RunnerCount; i++) //Instantiate new runners
                {
                    //Instantiate
                    GameObject opponent_Obj = GameObject.Instantiate(OpponentRunner_Prefab, RunnersParent_T);
                    opponent_Obj.GetComponent<Runner>().NewRace(this.Data.StartPos_T[i], this.CurrentLevel.FinishPos_T);
                    this.Data.Runners_T.Add(opponent_Obj.transform);
                    opponent_Obj.SetActive(true);
                }
            }
        }

        public void RunnerHitObstacle(Transform runner_T)
        {
            if(this.Data.Player_T == runner_T)
            {
                SoundManager.Instance.PlaySound("RunnerHit");
            }

            if(this.Data.CurrentGameMode == GameMode.Single)
            {
                GameManager.Instance.RaceLost();
            }
            else
            {
                this.TeleportRunnerToStart(runner_T);
            }
        }

        public void RunnerFinishedRace(Transform runner_T)
        {
            if (this.Data.CurrentGameMode == GameMode.Single)
            {
                GameManager.Instance.RaceWon();
            }
            else if(this.Data.CurrentGameMode == GameMode.PVE)
            {
                if(runner_T == this.Data.Player_T)
                {
                    if(this.Data.PlayerPlace == 1)
                    {
                        GameManager.Instance.RaceWon();
                    }
                    else
                    {
                        GameManager.Instance.RaceLost();
                    }
                }
                else
                {
                    runner_T.GetComponent<OpponentRunner>().FinishedRace();
                }
            }
        }



        private void TeleportRunnerToStart(Transform runner_T)
        {
            runner_T.GetComponent<Runner>().GoToStartPosition();
        }
    }
}
