using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunnerBoi.Character
{
    public class OpponentRunner : Runner
    {
        [SerializeField] GameObject RunnerObj;
        private Animator runnerAnimator;

        private void Awake()
        {
            this.SetDefaults();
            runnerAnimator = RunnerObj.GetComponent<Animator>();
        }

        private void Start()
        {
            Actions.Instance.OnGameStateChange += HandleGameStateChange;
        }

        protected override void SetDefaults()
        {
            StopAllCoroutines();
            base.SetDefaults();
            StartCoroutine(this.NMASettings());
        }

        //public override void NewRace(Transform startPos_T, Transform finishPos_T)
        //{
        //    base.NewRace(startPos_T, finishPos_T);
        //}

        private IEnumerator NMASettings()
        {
            yield return new WaitForFixedUpdate();
            base.NMAgent.enabled = true;
            if (base.NMAgent.isActiveAndEnabled)
            {
                Vector3 finishPos = base.FinishPos_T.position;
                finishPos.x = this.transform.position.x;
                base.NMAgent.destination = finishPos;
                base.NMAgent.isStopped = true;
            }
        }

        public void FinishedRace()
        {
            Vector3 finishDest = this.transform.position;
            finishDest.x = base.StartPos_T.position.x;
            finishDest.z += 25f;
            base.NMAgent.destination = finishDest;
            StartCoroutine(IsArrived());
        }

        private IEnumerator IsArrived()
        {
            while(true)
            {
                yield return new WaitForFixedUpdate();
                if(base.NMAgent.remainingDistance < 5f)
                {
                    runnerAnimator.SetFloat("RunSpeed", 0);
                    base.NMAgent.isStopped = true;
                    break;
                }
            }
        }

        private void HandleGameStateChange(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.OnReady:
                    break;
                case GameState.Racing:
                    NMAgent.isStopped = false;
                    runnerAnimator.SetFloat("RunSpeed", 1);
                    break;
                case GameState.RaceFinished:
                    NMAgent.isStopped = true;
                    runnerAnimator.SetFloat("RunSpeed", 0);
                    break;
                case GameState.Wallpaint:
                    break;
                default:
                    break;
            }
        }
    }
}

